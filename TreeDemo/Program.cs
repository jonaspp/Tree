using System;
using System.Collections.Generic;
using System.Text;
using Tree;
using Tree.Daemon;

namespace TreeDemo
{
    public class Program
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            DaemonRunner.Run(args);
        }
    }
}
