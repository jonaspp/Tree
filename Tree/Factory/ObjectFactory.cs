﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Lifecycle;
using Tree.Injector;
using Tree.Container;
using System.Runtime.Serialization;
using Tree.Configuration;
using System.Configuration;

namespace Tree.Factory
{
    public class ObjectFactory
    {
        public static T Create<T>(params object[] parameters) where T : class
        {
            Type type = typeof(T);
            object obj = Create(type, parameters);
            return obj as T;
        }

        public static Type ImplFor(Type type)
        {
            string interfaceName = string.Format("{0}.Impl.{1}Impl", type.Namespace, type.Name.Substring(1, type.Name.Length - 1));
            Type classType = ObjectFactory.TypeFrom(interfaceName);
            if (classType == null)
            {
                throw new NotImplementedException();
            }
            return classType;
        }

        public static object Create(Type type, params object[] parameters)
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

            if (obj is IConfigure)
            {
                ContainerElement el = ObjectConfiguration.ConfigurationFor(type); 
                ((IConfigure)obj).Configure(el);
            }
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

        public static Type TypeFrom(string type)
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
