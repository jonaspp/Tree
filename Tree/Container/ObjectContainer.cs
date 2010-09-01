using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace Tree.Container
{
    public class ObjectContainer
    {
        public static Dictionary<string, object> Objects
        {
            get;
            private set;
        }

        static ObjectContainer()
        {
            Objects = new Dictionary<string, object>();
        }

        public static void Start()
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

        public static void Stop()
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
    }
}
