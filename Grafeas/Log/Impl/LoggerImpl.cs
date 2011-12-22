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

namespace Tree.Grafeas.Impl
{
    public class LoggerImpl : ILogger, IStartable, IInitializable
    {
        private DateTime lastRoll = DateTime.Now;

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
                    LogEntry entry = logQueue.Dequeue();
                    ILogAppender appender = AppenderFor(entry);
                    if (appender != null)
                    {
                        if (DateTime.Now > lastRoll.AddHours(12))
                        {
                            appender.Roll();
                        }
                        appender.Write(entry.ToString(appender.Pattern));
                    }
                    else
                    {
                        Console.WriteLine(entry.ToString(null));
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    hasEntriesSignal.WaitOne();
                }
            }
        }

        private ILogAppender AppenderFor(LogEntry entry)
        {
            int x = 0, i;
            ILogAppender appender = null;
            string[] a1 = entry.Namespace.Split('.');
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
                    appender = appenders[str];
                }
            }
            return appender == null ? defaultAppender : appender;
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

        public void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

        public void Error(string message, params object[] args)
        {
            Error(string.Format(message, args));
        }

        public void Debug(string message, params object[] args)
        {
            Debug(string.Format(message, args));
        }

        public void Warn(string message, params object[] args)
        {
            Warn(string.Format(message, args));
        }

        private void Log(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Info));
        }

        private void Error(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Error));
        }

        private void Debug(string message)
        {
            ProcessWrite(new LogEntry(message, LogEntry.Level.Debug));
        }

        private void Warn(string message)
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
