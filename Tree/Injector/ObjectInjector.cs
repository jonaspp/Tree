using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Factory;

namespace Tree.Injector
{
    public class ObjectInjector
    {
        public static void Inject(object obj)
        {
            Type objType = obj.GetType();
            foreach (FieldInfo field in objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    if (attribute is Inject)
                    {
                        field.SetValue(obj, ObjectFactory.Get(field.FieldType, ((Inject)attribute).Parameters));
                    }
                }
            }
        }
    }
}