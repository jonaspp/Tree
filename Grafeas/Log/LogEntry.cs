using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Tree.Grafeas.Impl;
using System.Collections;

namespace Tree.Grafeas
{
    public class LogEntry
    {
        public enum Level {Debug, Info, Warning, Error, Fatal};

        public Level LogLevel { get; set; }

        public DateTime Date { get; set; }

        [LogFieldAttribute("t")]
        public string ThreadName { get; set; }

        [LogFieldAttribute("d")]
        public string DateString { get { return DateTime.Now.ToString("dd/MM/yy HH:mm:ss"); } }

        [LogFieldAttribute("l")]
        public string LogLevelString { get { return LogLevel.ToString().ToUpper(); } }

        [LogFieldAttribute("s")]
        public string Detail { get; set; }

        [LogFieldAttribute("n")]
        public string Namespace { get; set; }

        [LogFieldAttribute("c")]
        public string TypeName { get; set; }

        [LogFieldAttribute("m")]
        public string MethodName { get; set; }

        [LogFieldAttribute("w")]
        public string Who { get; set; }

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        internal LogEntry()
        {
        }

        internal LogEntry(string detail, Level level)
        {
            this.Detail = detail;
            this.ThreadName = Thread.CurrentThread.Name;
            this.LogLevel = level;
            this.Date = DateTime.Now;

            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(2).GetMethod();
            this.MethodName = methodBase.Name;
            this.Namespace = methodBase.DeclaringType.Namespace;
            this.TypeName = methodBase.DeclaringType.Name;
            this.Who = string.Format("{0}.{1}.{2}()", Namespace, TypeName, MethodName);
        }
        
        private string GetPattern(string pattern)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                foreach (object attribute in prop.GetCustomAttributes(true))
                {
                    if (attribute is LogFieldAttribute)
                    {
                        string val = (prop.GetValue(this, null) == null) ? "" : prop.GetValue(this, null).ToString();
                        string p = ((LogFieldAttribute)attribute).Pattern;
                        
                        pattern = pattern.Replace("{" + p + "}", val);
                    }
                }
            }
            pattern = pattern.Replace("\\r", "\r");
            pattern = pattern.Replace("\\t", "\t");
            pattern = pattern.Replace("\\n", "\n");
            return pattern;
        }

        public string ToString(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return string.Format("{0}\t{1}  [{2}]  {3}\r\n{4}\r\n", LogLevel.ToString().ToUpper(), DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), ThreadName, Who, Detail);
            }
            else
            {
                return GetPattern(pattern);
            }
        }
    }
}
