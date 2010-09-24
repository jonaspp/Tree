using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Log.Impl
{
    public class ConsoleLogger : ILogAppender
    {
        public void Write(LogEntry entry)
        {
            Console.WriteLine(entry.ToString());
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public string Name
        {
            get { return "ConsoleLogger"; }
        }
    }
}
