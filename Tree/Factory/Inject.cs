using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Factory
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class Inject : Attribute
    {
        private object[] Parameters { get; set; }
        public Inject(params object [] parameters)
        {
            Parameters = parameters;
        }
    }
}
