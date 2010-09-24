using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Log
{
    public interface ILogger
    {
        void Log(Exception ex);

        void Log(string message, params object [] p);

        void Log(LogEntry.Level level, string message, params object[] p);

        void Log(string message);

        void Log(string message, LogEntry.Level level);

        void Finish();
    }
}
