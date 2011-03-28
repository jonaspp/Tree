using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Factory;
using Tree.Container;

namespace Tree.Injector
{
    public class ObjectInjector
    {
        public static void Inject(object obj)
        {
            Inject(obj, obj.GetType());
        }

        public static void Inject(object obj, Type objType)
        {
            foreach (FieldInfo field in objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    if (attribute is Inject)
                    {
                        field.SetValue(obj, ObjectContainer.Lookup(field.FieldType, ((Inject)attribute).Parameters));
                    }
                }
            }
        }
    }
}