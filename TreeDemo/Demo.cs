using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tree.Factory;
using Tree.Runner;
using Tree;
using Utils;

namespace TreeDemo
{
    class Demo : IEntryPoint
    {
        public void Start(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Core.Factory.Create<FormMain>());
        }

        public void Stop()
        {
        }

        public void WaitForExit()
        {
        }
    }
}
