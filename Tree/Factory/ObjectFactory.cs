using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Lifecycle;
using Tree.Injector;
using Tree.Container;

namespace Tree.Factory
{
    public class ObjectFactory
    { 
        public static T Get<T>(params object [] parameters) where T : class
        {
            if (typeof(T).IsInterface)
            {
                Type interfaceType = typeof(T);
                return Get(interfaceType, parameters) as T;
            }
            throw new NotImplementedException();
        }

        public static object Get(Type type, params object [] parameters)
        {
            if (ObjectContainer.Objects.ContainsKey(type.FullName))
            {
                object obj = ObjectContainer.Objects[type.FullName];
                if (obj is IReset)
                {
                    ((IReset)obj).Reset();
                }
                return obj;
            }
            else
            {
                string interfaceName = string.Format("{0}.Impl.{1}Impl", type.Namespace, type.Name.Substring(1, type.Name.Length - 1));
                Type classType = Type.GetType(interfaceName);
                if (classType == null)
                {
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        string implName = string.Format("{0}, {1}", interfaceName, assembly.GetName());
                        classType = Type.GetType(implName);
                        if (classType != null)
                        {
                            break;
                        }
                    }
                }
                if (classType == null)
                {
                    throw new NotImplementedException();
                }
                return Register(type, classType, parameters);
            }
        }

        public static T Register<T, C>(params object[] parameters) where T : class
        {
            if (typeof(T).IsInterface)
            {
                Type interfaceType = typeof(T);
                return Register(interfaceType, typeof(C), parameters) as T;
            }
            throw new NotImplementedException();
        }

        private static object Register(Type role, Type impl, params object[] parameters)
        {
            Type[] parametersType = new Type[0];
            if (parameters != null)
            {
                parametersType = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parametersType[i] = parameters[i].GetType();
                }
            }
            object obj = impl.GetConstructor(parametersType).Invoke(parameters);

            ObjectInjector.Inject(obj);

            if (obj is IInitialize)
            {
                ((IInitialize)obj).Initilize();
            }
            ObjectContainer.Objects.Add(role.FullName, obj);
            return ObjectContainer.Objects[role.FullName];
        }
    }
}
