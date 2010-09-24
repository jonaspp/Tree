using System.Threading;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace Tree.Log.Impl
{
    public class LoggerImpl : ILogger
    {       
        private ILogAppender logger;

        public LoggerImpl()
        {
            if (logger == null)
            {
                logger = new FileLogger();
            }
            logger.Start();
        }

        public void Log(Exception ex)
        {
            LogEntry entry = new LogEntry(ex.Source + ": " + ex.Message + ", at " + ex.StackTrace, LogEntry.Level.Error);
            Write(entry);
        }

        public void Log(string message, params object [] p)
        {
            Log(string.Format(message, p));
        }

        public void Log(LogEntry.Level level, string message, params object[] p)
        {
            Log(string.Format(message, p), level);
        }

        public void Log(string message)
        {
            LogEntry entry = new LogEntry(message, LogEntry.Level.Info);
            Write(entry);            
        }

        public void Log(string message, LogEntry.Level level)
        {
            LogEntry entry = new LogEntry(message, level);
            Write(entry);
        }
        
        private void Write(LogEntry entry)
        {
            logger.Write(entry);
        }

        public void Finish()
        {
            logger.Stop();
        }
    }
}
