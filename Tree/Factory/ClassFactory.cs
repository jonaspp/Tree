using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tree.Factory
{
    public class ObjectFactory
    {        
        private static Dictionary<string, object> container = new Dictionary<string, object>();

        public static T Get<T>(params object [] parameters) where T : class
        {
            if (typeof(T).IsInterface)
            {
                Type interfaceType = typeof(T);
                return Get(interfaceType, parameters) as T;
            }
            throw new NotImplementedException();
        }

        private static object Get(Type type, params object [] parameters)
        {
            if (container.ContainsKey(type.FullName))
            {
                object obj = container[type.FullName];
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
            foreach (FieldInfo field in impl.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    if (attribute is Inject)
                    {
                        field.SetValue(obj, Get(field.FieldType));
                    }
                }
            }
            if (obj is IInitialize)
            {
                ((IInitialize)obj).Initilize();
            }
            container.Add(role.FullName, obj);
            return container[role.FullName];
        }

        public static void Start()
        {
            foreach (string key in container.Keys)
            {
                object obj = container[key];
                if (obj is IStart)
                {
                    ((IStart)obj).Start();
                }
            }
        }

        public static void Stop()
        {
            foreach (string key in container.Keys)
            {
                object obj = container[key];
                if (obj is IStart)
                {
                    ((IStart)obj).Stop();
                }
            }
        }
    }
}
