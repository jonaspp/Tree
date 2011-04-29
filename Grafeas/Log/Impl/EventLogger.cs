using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Tree.Grafeas.Impl
{
    public class EventLogger : ILogAppender
    {
        private EventLogger() 
        { 
        }

        public void Write(string str)
        {
            EventLog.WriteEntry(Name, str, EventLogEntryType.Information);
        }

        public string Name
        {
            get;
            set;
        }

        public string Pattern
        {
            get;
            set;
        }

        public void Setup()
        {
        }

        public string Path
        {
            get;
            set;
        }
    }
}
