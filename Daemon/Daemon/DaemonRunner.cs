using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Tree.Process;
using System.Security.Principal;
using System.ServiceProcess;
using Tree.Factory;
using Tree.Container;
using System.Reflection;
using Tree.Grafeas;

namespace Tree.Daemon
{
    public class DaemonRunner
    {
        const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        private static ILogger logger = ObjectContainer.Lookup<ILogger>();
        
        [STAThread()]
        static void Main(string[] args)
        {
            bool isService = true;
            bool isSilent = false;
            string type = string.Empty;
            string assembly = string.Empty;
            IWrappedDaemon daemon = null;

            string argsString = "(";
            foreach (string str in args)
            {
                argsString += str + ","; 
            }
            argsString = argsString.TrimEnd(',') + ")";
            logger.Debug("Starting DaemonRunner with args {0}", argsString);

            System.Diagnostics.Process parentProcess = ProcessHelper.GetParentProcess();

            foreach (string str in args)
            {
                if (str.ToLower().Contains("silent"))
                {
                    isSilent = true;
                }
                if (str.ToLower().Contains("assembly="))
                {
                    assembly = str.Split('=')[1];
                }
                if (str.ToLower().Contains("daemon="))
                {
                    type = str.Split('=')[1];
                }
            }

            if(string.IsNullOrEmpty(assembly))
            {
                logger.Error("Could not find {0}", assembly);
                throw new Exception("Could not find " + assembly);
            }

            if (string.IsNullOrEmpty(type))
            {
                logger.Error("Could not create {0}", type);
                throw new Exception("Could not create " + type);
            }

            AppDomain.CurrentDomain.Load(assembly);

            Type t = ObjectFactory.TypeFrom(type);
            daemon = ObjectFactory.Create(t) as IWrappedDaemon;

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            isService &= parentProcess.ProcessName.ToLower().Contains("services");
            isService &= identity.IsSystem;

            ObjectContainer.Static.Start();
            if (!isSilent)
            {
                if (parentProcess.ProcessName.ToLower().Contains("cmd"))
                {
                    AttachConsole(ATTACH_PARENT_PROCESS);
                }
                else
                {
                    AllocConsole();
                }
            }
            if (isService)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
    				new WrappedService(daemon)
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                daemon.Start(args);
                daemon.WaitForExit();
                daemon.Stop();

                if (!isSilent)
                {
                    FreeConsole();
                }
            }
            ObjectContainer.Static.Stop();
        }
    }
}
