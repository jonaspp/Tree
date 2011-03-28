using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Tree.Configuration
{
    public class ObjectConfiguration
    {
        private static ContainerConfiguration config = (ContainerConfiguration)ConfigurationManager.GetSection("Container");

        private static ContainerElement GetContainerElement(string name)
        {
            foreach (ContainerElement el in config.Collection)
            {
                if (el.Impl.Equals(name))
                {
                    return el;
                }
            }
            return null;
        }

        public static ContainerElement ConfigurationFor(Type t)
        {
            return GetContainerElement(t.FullName);
        }
            
    }
}
