using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tree;

namespace TreeDemo
{
    static class Program
    {
        [STAThread]
        static void Main(string [] args)
        {
            Runner.ShowForm(typeof(FormMain), args);
        }
    }
}
