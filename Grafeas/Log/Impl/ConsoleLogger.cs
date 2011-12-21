using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Grafeas.Impl
{
    public class ConsoleLogger : ILogAppender
    {
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

        public void Write(string message)
        {
           Console.Out.WriteLine(message);
        }

        public string Path
        {
            get;
            set;
        }

        public void Roll()
        {
        }
    }
}
