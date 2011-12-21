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

        private static bool isService = true;
        private static bool isSilent = false;
                
        public static void Run(string[] args)
        {
            Type typeClass = null;
            Assembly caller = Assembly.GetCallingAssembly();           
            IWrappedDaemon daemon = null;
            
            foreach (Type t in caller.GetTypes())
            {
                foreach (Type i in t.GetInterfaces())
                {
                    if (i == typeof(IWrappedDaemon))
                    {
                        typeClass = t;
                    }
                }
            }
            if (typeClass == null)
            {
                typeClass = TypeFromArgs(args);
            }

            daemon = ObjectFactory.Create(typeClass) as IWrappedDaemon;
            System.Diagnostics.Process parentProcess = ProcessHelper.GetParentProcess();
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
            }
            ObjectContainer.Static.Stop();
            if (!isSilent)
            {
                FreeConsole();
            }
        }

        private static Type TypeFromArgs(string[] args)
        {            
            string type = string.Empty;
            string assembly = string.Empty;
            string argsString = "(";
            foreach (string str in args)
            {
                argsString += str + ",";
            }
            argsString = argsString.TrimEnd(',') + ")";
            logger.Debug("Starting DaemonRunner with args {0}", argsString);

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

            if (string.IsNullOrEmpty(assembly))
            {
                logger.Error("Assembly cannot be null.");
                throw new Exception("Assembly cannot be null.");
            }

            if (string.IsNullOrEmpty(type))
            {
                logger.Error("Daemon type cannot be null.");
                throw new Exception("Daemon type cannot be null.");
            }
            AppDomain.CurrentDomain.Load(assembly);
            return ObjectFactory.TypeFrom(type);
        }
    }
}
