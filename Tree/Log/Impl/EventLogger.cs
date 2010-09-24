using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Tree.Log.Impl
{
    public class EventLogger : ILogAppender
    {
        private string name;

        private EventLogger() { }

        public EventLogger(string name) 
        {
            this.name = name;
        }

        public void Write(LogEntry entry)
        {
            EventLog.WriteEntry(name, entry.ToString(), EventLogEntryType.Information);
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public string Name
        {
            get { return name; }
        }
    }
}
