using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Tree.Lifecycle;
using System.Configuration;

namespace Tree.Grafeas.Impl
{
    public class FileLogger : ILogAppender
    {
        private StreamWriter logFile = null;

        public FileLogger()
        {
                    
        }

        public void Write(string str)
        {
            logFile.WriteLine(str);
            logFile.Flush();
        }

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

        public string Path
        {
            get;
            set;
        }

        public void Setup()
        {
            if (string.IsNullOrEmpty(Path))
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    Name = Assembly.GetCallingAssembly().GetName().Name.ToLower();
                }
                Path = Environment.GetEnvironmentVariable("Appdata") + "\\" + this.Name + "\\";
                Path = Path + this.Name + "-" + Environment.UserName.Trim().ToLower() + ".log";
            }
            else
            {
                Path = Environment.ExpandEnvironmentVariables(Path);
            }
            FileInfo info = new FileInfo(Path);
            if (!info.Directory.Exists)
            {
                info.Directory.Create();
            }
            logFile = File.AppendText(Path);
        }
    }
}
