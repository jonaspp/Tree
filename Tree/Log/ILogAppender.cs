using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Log
{
    public interface ILogAppender
    {
        string Name { get; }

        void Start();

        void Write(LogEntry entry);

        void Stop();
    }
}
