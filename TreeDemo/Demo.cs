using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tree.Factory;
using Tree.Daemon;

namespace TreeDemo
{
    class Demo : IWrappedDaemon
    {
        public void Start(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ObjectFactory.Create<FormMain>());
        }

        public void Stop()
        {
        }

        public void WaitForExit()
        {
        }
    }
}
