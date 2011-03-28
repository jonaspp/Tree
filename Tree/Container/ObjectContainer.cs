using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;
using Tree.Configuration;
using Tree.Factory;
using System.Configuration;

namespace Tree.Container
{
    public class ObjectContainer : IStart
    {        
        public static ObjectContainer Static { get; private set; }

        static ObjectContainer()
        {
            Static = new ObjectContainer();
            ObjectContainer.Static.Start();
        }

        public Dictionary<string, object> Objects
        {
            get;
            private set;
        }

        public ObjectContainer()
        {
            Objects = new Dictionary<string, object>();
        }

        public void Start()
        {
            Initialize();

            foreach (string key in Objects.Keys)
            {
                object obj = Objects[key];
                if (obj is IStart)
                {
                    ((IStart)obj).Start();
                }
            }
        }

        public void Stop()
        {
            foreach (string key in Objects.Keys)
            {
                object obj = Objects[key];
                if (obj is IStart)
                {
                    ((IStart)obj).Stop();
                }
            }
        }

        private void Initialize()
        {
            ContainerConfiguration config = (ContainerConfiguration)ConfigurationManager.GetSection("Container");
            if (config != null)
            {
                foreach (ContainerElement element in config.Collection)
                {
                    Type t = ObjectFactory.TypeFrom(element.Type);
                    Type i = ObjectFactory.TypeFrom(element.Impl);
                    Register(t, i);
                }
            }
        }

        public static T Get<T>(params object[] parameters) where T : class
        {
            Type interfaceType = typeof(T);
            return Get(interfaceType, parameters) as T;
        }

        internal static object Get(Type type, params object[] parameters)
        {
            if (type.IsInterface)
            {
                if (Static.Objects.ContainsKey(type.FullName))
                {
                    object obj = Static.Objects[type.FullName];
                    if (obj is IReset)
                    {
                        ((IReset)obj).Reset();
                    }
                    return obj;
                }
            }
            return null;
        }        

        public static T Register<T, C>(params object[] parameters) where T : class
        {            
            T obj = Get<T>(parameters);
            if (obj == null)
            {
                if (typeof(T).IsInterface)
                {
                    Type interfaceType = typeof(T);
                    return Register(interfaceType, typeof(C), parameters) as T;
                }
            }
            else
            {
                return obj;
            }
            throw new NotImplementedException();
        }

        private static object Register(Type role, Type impl, params object[] parameters)
        {
            object obj = ObjectFactory.Create(impl, parameters);
            Static.Objects.Add(role.FullName, obj);
            return obj;
        }

        public static T Lookup<T>(params object[] parameters) where T : class
        {
            return Lookup(typeof(T), parameters) as T;
        }

        public static object Lookup(Type type, params object[] parameters)
        {
            object obj = Get(type, parameters);
            if (obj == null)
            {
                if (type.IsInterface)
                {
                    Type impl = ObjectFactory.ImplFor(type);
                    return Register(type, impl, parameters);
                }
            }
            else
            {
                return obj;
            }
            throw new NotImplementedException();
        }
    }
}
