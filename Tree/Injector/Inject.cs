using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Injector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class Inject : Attribute
    {
        public object[] Parameters { get; set; }
        public Inject(params object [] parameters)
        {
            Parameters = parameters;
        }
    }
}
