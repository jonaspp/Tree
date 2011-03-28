using System.Threading;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Tree.Lifecycle;
using Tree.Configuration;
using System.Configuration;
using Tree.Factory;

namespace Tree.Log.Impl
{
    public class LoggerImpl : ILogger, IStart, IInitialize
    {       
        private const int THREAD_TIMEOUT_JOIN = 3000;
        private Thread workerThread;

        protected bool running;
        protected AutoResetEvent hasEntriesSignal = new AutoResetEvent(false);
        protected Queue<LogEntry> logQueue = new Queue<LogEntry>();

        private Dictionary<string, ILogAppender> appenders = new Dictionary<string, ILogAppender>();

        private ILogAppender defaultAppender = null;

        public LoggerImpl()
        {
        }

        public void Start()
        {
            if (!running)
            {
                running = true;
                workerThread = new Thread(new ThreadStart(WorkerProc));
                workerThread.Name = "TreeLogger";
                workerThread.Start();
            }
        }

        private void WorkerProc()
        {
            while (running || logQueue.Count > 0)
            {
                hasEntriesSignal.Reset();
                if (logQueue.Count > 0)
                {
                    LogEntry e = logQueue.Dequeue();
                    ILogAppender a = AppenderFor(e);
                    if (a != null)
                    {
                        a.Write(e.ToString(a.Pattern));
                    }
                    else
                    {
                        Console.WriteLine(e.ToString(null));
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    hasEntriesSignal.WaitOne();
                }
            }
        }

        private ILogAppender AppenderFor(LogEntry e)
        {
            int x = 0, i;
            ILogAppender ap = null;
            string[] a1 = e.Namespace.Split('.');
            foreach(string str in appenders.Keys)
            {
                string[] a2 = str.Split('.');

                int l = -1;
                if (a1.Length > a2.Length)
                {
                    l = a2.Length;
                }
                else
                {
                    l = a1.Length;
                }
                i = 0;
                for (i = 0; i < l; i++)
                {
                    if (!a1[i].Equals(a2[i]))
                    {
                        break;
                    }
                }
                if ((i - (a2.Length - i)) > x)
                {
                    x = i;
                    ap = appenders[str];
                }
            }

            return ap == null ? defaultAppender : ap;
        }

        public void Stop()
        {
            if (running)
            {
                while (logQueue.Count > 0)
                {
                    Thread.Sleep(10);
                }

                running = false;
                hasEntriesSignal.Set();
                if (!workerThread.Join(THREAD_TIMEOUT_JOIN))
                {
                    workerThread.Abort();
                }
            }
        }

        public void Log(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Info));
        }

        public void Error(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Error));
        }

        public void Debug(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Debug));
        }

        public void Warn(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Warning));
        }

        private void ProcessWrite(LogEntry entry)
        {
            logQueue.Enqueue(entry);
            hasEntriesSignal.Set();
        }

        public void Initialize()
        {
            LoggerConfiguration config = (LoggerConfiguration)ConfigurationManager.GetSection("Logger");
            if (config != null)
            {
                foreach (AppenderElement app in config.Appenders)
                {
                    ILogAppender appender = (ILogAppender)ObjectFactory.Create(Type.GetType(app.Type));
                    appender.Pattern = app.Pattern;
                    appender.Name = app.Name;
                    appender.Path = app.Path;
                    appender.Setup();
                    if (app.Name.Equals("default"))
                    {
                        defaultAppender = appender;
                    }
                    else
                    {
                        appenders.Add(app.Namespace, appender);
                    }
                }
            }
            else
            {
                Console.Out.WriteLine("*** No Log Appenders were found, loggin on stdout.");
                defaultAppender = new ConsoleLogger();
                defaultAppender.Name = "default";
                defaultAppender.Pattern = "{l}\t{d}  [{t}]\t{w}:\r\n{s}\r\n";
            }
        }
    }
}
