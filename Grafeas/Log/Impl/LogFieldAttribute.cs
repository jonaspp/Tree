using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Grafeas.Impl
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LogFieldAttribute : Attribute
    {
        public string Pattern
        {
            get;
            private set;
        }
        public LogFieldAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
