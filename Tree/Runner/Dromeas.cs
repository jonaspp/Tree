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
namespace Tree.Runner
{
    public class Dromeas
    {
        const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        private static bool isService = true;
        private static bool isSilent = false;
                
        public static void Run(string[] args)
        {
            Type typeClass = null;
            Assembly caller = Assembly.GetEntryAssembly();           
            IEntryPoint daemon = null;
            
            foreach (Type t in caller.GetTypes())
            {
                foreach (Type i in t.GetInterfaces())
                {
                    if (i == typeof(IEntryPoint))
                    {
                        typeClass = t;
                    }
                }
            }
            if (typeClass == null)
            {
                typeClass = TypeFromArgs(args);
            }

            daemon = Core.Factory.Create(typeClass) as IEntryPoint;
            System.Diagnostics.Process parentProcess = ProcessHelper.GetParentProcess();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            isService &= parentProcess.ProcessName.ToLower().Contains("services");
            isService &= identity.IsSystem;

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
            Core.Container.Stop();
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
                throw new Exception("Assembly cannot be null.");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Daemon type cannot be null.");
            }
            AppDomain.CurrentDomain.Load(assembly);
            return Core.Factory.TypeFrom(type);
        }
    }
}
