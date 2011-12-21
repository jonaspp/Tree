using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Tree.Lifecycle;
using System.Configuration;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace Tree.Grafeas.Impl
{
    public class FileLogger : ILogAppender
    {
        private FileInfo file = null;

        private StreamWriter logFile = null;

        public FileLogger()
        {                   
        }

        public void Write(string str)
        {
            Console.Out.WriteLine(str);
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
            file = new FileInfo(Path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (file.Exists)
            {
                Roll();
            }
            logFile = File.AppendText(Path);
        }

        public void Roll()
        {           
            if (OlderThanOneDay() || MoreThanOneMB())
            {
                bool reopen = false;
                if (logFile != null)
                {
                    logFile.Flush();
                    logFile.Close();
                    logFile = null;
                    reopen = true;
                }
                ZipLog();                
                file.Delete();
                if (reopen)
                {
                    logFile = File.AppendText(Path);
                }
            }
        }

        private string ZipLog()
        {
            string filename = file.FullName;
            string zipFile = filename + "." + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".zip";
            FileStream stream = File.Create(zipFile);
            ZipOutputStream zipStream = new ZipOutputStream(stream);
            zipStream.SetLevel(5);

            FileInfo fi = new FileInfo(filename);
            ZipEntry newEntry = new ZipEntry(fi.Name);
            newEntry.DateTime = fi.LastWriteTime;
            newEntry.Size = fi.Length;
            zipStream.PutNextEntry(newEntry);
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(filename))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
            zipStream.CloseEntry();
            zipStream.IsStreamOwner = true;
            zipStream.Close();
            return zipFile;
        }

        private bool OlderThanOneDay()
        {
            DateTime now = DateTime.Now;
            DateTime last = file.LastWriteTime;
            return now.Subtract(last).Hours > 24;
        }

        private bool MoreThanOneMB()
        {
            Stream s = new FileStream(file.FullName, FileMode.Open);
            bool moreThanOneMB = s.Length > (1024 * 1024);
            s.Close();
            return moreThanOneMB;
        }
    }
}
