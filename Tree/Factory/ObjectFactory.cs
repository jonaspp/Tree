using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Lifecycle;
using Tree.Injector;
using Tree.Container;
using System.Runtime.Serialization;

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
            else
            {
                object obj = Create(typeof(T), parameters);
                return obj as T;
            }
        }

        private static object Create(Type type, params object[] parameters)
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
            object obj = FormatterServices.GetUninitializedObject(type);
            ObjectInjector.Inject(obj, type);
            type.GetConstructor(parametersType).Invoke(obj, parameters);

            if (obj is IInitialize)
            {
                ((IInitialize)obj).Initialize();
            } 
            if (obj is IStart)
            {
                ((IStart)obj).Start();
            }
            return obj;
        }

        internal static object Get(Type type, params object [] parameters)
        {
            if (type.IsInterface)
            {
                if (ObjectContainer.StaticInstance.Objects.ContainsKey(type.FullName))
                {
                    object obj = ObjectContainer.StaticInstance.Objects[type.FullName];
                    if (obj is IReset)
                    {
                        ((IReset)obj).Reset();
                    }
                    return obj;
                }
                else
                {
                    string interfaceName = string.Format("{0}.Impl.{1}Impl", type.Namespace, type.Name.Substring(1, type.Name.Length - 1));
                    Type classType = GetTypeFrom(interfaceName);
                    if (classType == null)
                    {
                        throw new NotImplementedException();
                    }
                    return Register(type, classType, parameters);
                }
            }
            else
            {
                object obj = Create(type, parameters);
                return obj;
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

        public static object Register(Type role, Type impl, params object[] parameters)
        {
            object obj = Create(impl, parameters);
            ObjectContainer.StaticInstance.Objects.Add(role.FullName, obj);
            return ObjectContainer.StaticInstance.Objects[role.FullName];
        }

        public static Type GetTypeFrom(string type)
        {
            Type classType = Type.GetType(type);
            if (classType == null)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string implName = string.Format("{0}, {1}", type, assembly.GetName());
                    classType = Type.GetType(implName);
                    if (classType != null)
                    {
                        break;
                    }
                }
            }
            return classType;
        }
    }
}
