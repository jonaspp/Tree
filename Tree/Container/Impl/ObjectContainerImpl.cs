using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;
using Tree.Configuration;
using Tree.Factory;
using System.Configuration;

namespace Tree.Container.Impl
{
    class ObjectContainerImpl : IObjectContainer
    { 
        public Dictionary<string, object> Objects
        {
            get;
            private set;
        }

        public ObjectContainerImpl()
        {
            Objects = new Dictionary<string, object>();
        }

        public void Start()
        {
            ContainerConfiguration config = (ContainerConfiguration)ConfigurationManager.GetSection("Container");
            if (config != null)
            {
                foreach (ContainerElement element in config.Collection)
                {
                    Type t = Core.Factory.TypeFrom(element.Type);
                    Type i = Core.Factory.TypeFrom(element.Impl);
                    Register(t, i);
                }
            }
        }

        public void Stop()
        {
            foreach (string key in Objects.Keys)
            {
                object obj = Objects[key];
                if (obj is IStartable)
                {
                    ((IStartable)obj).Stop();
                }
            }
        }

        public T Get<T>() where T : class
        {
            Type interfaceType = typeof(T);
            return Get(interfaceType) as T;
        }

        public T Register<T, C>(params object[] parameters)
            where T : class
            where C : T
        {
            return Register(typeof(T), typeof(C), parameters) as T;
        }

        public T Lookup<T>(params object[] parameters) where T : class
        {
            return Lookup(typeof(T), parameters) as T;
        }

        internal object Get(Type type)
        {
            if (type.IsInterface)
            {
                if (Objects.ContainsKey(type.FullName))
                {
                    object obj = Objects[type.FullName];
                    return obj;
                }
            }
            return null;
        }       

        internal object Register(Type role, Type impl, params object[] parameters)
        {
            object obj = Get(role);
            if (obj == null)
            {
                if (role.IsInterface)
                {
                    obj = Core.Factory.Create(impl, parameters);
                    if (!Objects.ContainsKey(role.FullName))
                    {
                        Objects.Add(role.FullName, obj);
                    }
                    return obj;
                }
            }
            else
            {
                return obj;
            }
            throw new NotImplementedException();
            
        }
        
        public object Lookup(Type type, params object[] parameters)
        {
            object obj = Get(type);
            if (obj == null)
            {
                if (type.IsInterface)
                {
                    Type impl = Core.Factory.ImplFor(type);
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
