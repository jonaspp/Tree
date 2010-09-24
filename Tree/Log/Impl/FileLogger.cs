using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Tree.Log.Impl
{
    public class FileLogger : LogAppenderSupport
    {
        private StreamWriter logFile = null;

        private string name;
        private string file;
        private string path;

        public FileLogger()
        {
            if (string.IsNullOrEmpty(this.name))
            {
                this.name = Assembly.GetCallingAssembly().GetName().Name.ToLower();
            }
            path = Environment.GetEnvironmentVariable("Appdata") + "\\" + this.name + "\\";
            file = this.name + "-" + Environment.UserName.Trim().ToLower() + ".log";
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                info.Create();
            }
            logFile = File.AppendText(path + file);        
        }

        public override void Write(LogEntry entry)
        {
            lock (logQueue)
            {
                logQueue.Enqueue(entry);
                hasEntriesSignal.Set();
            }
        }

        public override string Name
        {
            get { return name; }
        }

        protected override void ProcessWrite(LogEntry entry)
        {
            logFile.WriteLine(entry.ToString());
            logFile.Flush();
        }
    }
}
