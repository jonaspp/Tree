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
        private static void Initialize()
        {
            ObjectContainer.Static.Start();
        }

        private static void Clean()
        {
            ObjectContainer.Static.Stop();
        }

        public static void ShowForm(Type formType, string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Initialize();            
            Form f = ObjectFactory.Create(formType) as Form;
            Application.Run(f);           
            Clean();
        }        

        public static void Run<T>(string[] args) where T : IInitialize
        {
            ObjectFactory.Create(typeof(T), args);
            Initialize();
            Clean();
        }
    }
}
