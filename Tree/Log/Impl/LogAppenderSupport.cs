using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tree.Log.Impl
{
    public abstract class LogAppenderSupport : ILogAppender
    {
        private const int THREAD_TIMEOUT_JOIN = 3000;
        private Thread workerThread;

        protected bool running;
        protected AutoResetEvent hasEntriesSignal = new AutoResetEvent(false);
        protected Queue<LogEntry> logQueue = new Queue<LogEntry>();

        public void Start()
        {
            if (!running)
            {
                running = true;
                workerThread = new Thread(new ThreadStart(WorkerProc));
                workerThread.Name = "Logger";
                workerThread.Start();
            }
        }

        public abstract void Write(LogEntry entry);

        protected abstract void ProcessWrite(LogEntry entry);

        private void WorkerProc()
        {
            while (running || logQueue.Count > 0)
            {
                hasEntriesSignal.Reset();
                if (logQueue.Count > 0)
                {
                    ProcessWrite(logQueue.Dequeue());
                    Thread.Sleep(100);
                }
                else
                {
                    hasEntriesSignal.WaitOne();
                }
            }
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                hasEntriesSignal.Set();
                if (!workerThread.Join(THREAD_TIMEOUT_JOIN))
                {
                    workerThread.Abort();
                }
            }
        }

        public abstract string Name { get; }
    }
}
