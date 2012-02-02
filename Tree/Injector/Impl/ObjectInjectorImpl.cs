using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Factory;
using Tree.Container;

namespace Tree.Injector.Impl
{
    class ObjectInjectorImpl : IObjectInjector
    {
        public void Inject(object obj)
        {
            Inject(obj, obj.GetType());
        }

        public void Inject(object obj, Type objType)
        {
            foreach (FieldInfo field in objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    if (attribute is Inject)
                    {
                        Type t = field.FieldType;
                        field.SetValue(obj, Core.Container.Lookup(t, ((Inject)attribute).Parameters));
                    }
                }
            }
        }
    }
}