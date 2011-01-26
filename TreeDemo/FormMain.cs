using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tree.Injector;
using Tree.Log;

namespace TreeDemo
{
    public partial class FormMain : Form
    {
        [Inject()]
        private ILogger logger;

        public FormMain()
        {
            logger.Debug("Running!");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int a = 10 / Convert.ToInt32("0");
        }
    }
}
