using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;
using Tree.Container;
using System.Windows.Forms;
using Tree.Factory;

namespace Tree
{
    public class Runner
    {
        public delegate int CustomRunnerHandler(string[] args);

        private static void Initialize()
        {
            ObjectContainer.StaticInstance.Initialize();
            ObjectContainer.StaticInstance.Start();
        }

        public static void ShowForm(Type formType, string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Initialize();            
            Form f = ObjectFactory.Get(formType) as Form;
            Application.Run(f);           
            Clean();
        }

        private static void Clean()
        {
            ObjectContainer.StaticInstance.Stop();
        }

        public static int Run(string[] args, CustomRunnerHandler handler)
        {
            if (handler == null)
            {
                throw new NotImplementedException();
            }
            ObjectContainer.StaticInstance.Initialize();
            ObjectContainer.StaticInstance.Start();
            int ret = handler(args);
            ObjectContainer.StaticInstance.Stop();
            return ret;
        }
    }
}
