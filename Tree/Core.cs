using System;
using System.Collections.Generic;
using System.Text;
using Tree.Container;
using Tree.Factory;
using Tree.Injector;
using Tree.Injector.Impl;
using Tree.Factory.Impl;
using Tree.Container.Impl;
using Tree.Runner;

namespace Tree
{
    public class Core
    {
        static Core()
        {
            injector = new ObjectInjectorImpl();
            factory = new ObjectFactoryImpl();            
            container = new ObjectContainerImpl();
            container.Start();
        }           
    
        private static IObjectContainer container;
        public static IObjectContainer Container
        {
            get
            {
                return container;
            }
        }

        private static IObjectFactory factory;
        public static IObjectFactory Factory
        {
            get
            {
                return factory;
            }
        }

        private static IObjectInjector injector;
        public static IObjectInjector Injector
        {
            get
            {
                return injector;
            }
        }

        public static void Run(string[] args)
        {
            Dromeas.Run(args);
        }
    }
}
