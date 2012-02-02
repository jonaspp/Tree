using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Lifecycle;
using Tree.Injector;
using Tree.Container;
using System.Runtime.Serialization;
using Tree.Configuration;
using System.Configuration;
using System.Globalization;

namespace Tree.Factory.Impl
{
    class ObjectFactoryImpl : IObjectFactory
    {
        public T Create<T>(params object[] parameters) where T : class
        {
            Type type = typeof(T);
            object obj = Create(type, parameters);
            return obj as T;
        }

        public Type ImplFor(Type type)
        {
            string interfaceName = string.Format("{0}.Impl.{1}Impl", type.Namespace, type.Name.Substring(1, type.Name.Length - 1));
            Type classType = TypeFrom(interfaceName);
            if (classType == null)
            {
                throw new NotImplementedException();
            }
            return classType;
        }

        public object Create(Type type, params object[] parameters)
        {
            if (type.IsInterface)
            {
                type = ImplFor(type);
            }
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
            Core.Injector.Inject(obj, type);
            if (obj is IConfigurable)
            {
                ObjectConfiguration.Configure(type, obj);
                ((IConfigurable)obj).Configure();
            }
            type.GetConstructor(parametersType).Invoke(obj, parameters);
            if (obj is IInitializable)
            {
                ((IInitializable)obj).Initialize();
            } 
            if (obj is IStartable)
            {
                ((IStartable)obj).Start();
            }
            return obj;
        }

        public Type TypeFrom(string type)
        {
            Type classType = Type.GetType(type);
            if (classType == null)
            {
                if (Assembly.GetEntryAssembly() == null)
                {
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        classType = TypeFromFullname(type, assembly.GetName().Name);
                        if (classType != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    classType = TypeFromFullname(type, Assembly.GetEntryAssembly().GetName().Name);
                    if (classType != null)
                    {
                        return classType;
                    }
                    foreach (AssemblyName assembly in Assembly.GetEntryAssembly().GetReferencedAssemblies())
                    {
                        classType = TypeFromFullname(type, assembly.Name);
                        if (classType != null)
                        {
                            break;
                        }
                    }
                }
            }
            return classType;
        }

        public Type TypeFromFullname(string type, string name)
        {
            string implName = string.Format("{0}, {1}", type, name);
            return Type.GetType(implName);
        }
    }
}
