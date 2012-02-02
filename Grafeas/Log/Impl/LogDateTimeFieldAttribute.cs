using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Grafeas.Impl
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LogDateTimeFieldAttribute : LogFieldAttribute
    {
        public string DateFormat
        {
            get;
            private set;
        }
        public LogDateTimeFieldAttribute(string name, string dateFormat)
            : base(name)
        {
            DateFormat = dateFormat;
        }
    }
}
