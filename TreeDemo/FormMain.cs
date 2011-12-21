using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tree.Injector;
using Tree.Grafeas;
using System.Threading;
using TreeDemo.Points;

namespace TreeDemo
{
    public partial class FormMain : Form
    {
        [Inject()]
        private ILogger logger;

        [Inject()]
        private IPointManager points;

        public FormMain()
        {
            Thread.CurrentThread.Name = "MAIN";
            logger.Debug("Running!");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Warn("CLICKED!!");
            IPoint p = points.Create(10, 20);
            points.Add(p);
            p = points.Create(11, 21);
            points.Add(p);
            p = points.Create(12, 22);
            points.Add(p);
            p = points.Create(13, 23);
            points.Add(p);
        }
    }
}
