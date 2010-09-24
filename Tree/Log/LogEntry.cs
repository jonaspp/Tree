using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Tree.Log
{
    public class LogEntry
    {
        public enum Level {Debug, Info, Warning, Error, Fatal};

        public DateTime Date { get; set; }
        public string ThreadName { get; set; }
        public Level LogLevel { get; set; }
        public string Detail { get; set; }

        public LogEntry()
        {
        }

        public LogEntry(string detail, Level level)
        {
            this.Detail = detail;
            this.ThreadName = Thread.CurrentThread.Name;
            this.LogLevel = level;
            this.Date = DateTime.Now;

            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(2).GetMethod();
            this.Who = methodBase.DeclaringType.FullName + "." + methodBase.Name + "()";
        }

        public string Who
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}  [{2}]  {3}\r\n{4}\r\n", LogLevel.ToString().ToUpper(), DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), ThreadName, Who, Detail);
        }

    }
}
