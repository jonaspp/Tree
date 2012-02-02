using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Tree.Lifecycle;
using System.Reflection;
using System.Globalization;

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

        public static void Configure(Type type, object obj)
        {
            ContainerElement el = ObjectConfiguration.ConfigurationFor(type);
            Dictionary<string, object> props = el.StateProperties;
            foreach (FieldInfo f in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (props.ContainsKey(f.Name))
                {
                    object val = ConvertTo(props[f.Name], f.FieldType);
                    f.SetValue(obj, val);
                }
            }
        }

        private static object ConvertTo(object p, Type type)
        {
            switch (type.ToString())
            {
                case "System.Int32":
                    return Convert.ToInt32(p);

                case "System.Boolean":
                    return Convert.ToBoolean(p);

                case "System.Double":
                    return Convert.ToDouble(p, CultureInfo.InvariantCulture);

                default:
                    return p;
            }
        }
    }
}
