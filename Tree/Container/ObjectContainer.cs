using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;
using Tree.Configuration;
using Tree.Factory;
using System.Configuration;

namespace Tree.Container
{
    public class ObjectContainer : IStart, IInitialize
    {        
        public static ObjectContainer StaticInstance { get; private set; }

        static ObjectContainer()
        {
            StaticInstance = new ObjectContainer();
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

        public void Initialize()
        {
            ContainerConfiguration config = (ContainerConfiguration)ConfigurationManager.GetSection("Container");
            if (config != null)
            {
                foreach (ContainerElement element in config.Collection)
                {
                    Type t = ObjectFactory.GetTypeFrom(element.Type);
                    Type i = ObjectFactory.GetTypeFrom(element.Impl);
                    ObjectFactory.Register(t, i);
                }
            }
        }
    }
}
