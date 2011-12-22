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

            LoadPoints();
        }

        private void LoadPoints()
        {
            foreach (IPoint p in points.Points())
            {
                logger.Log("Loading '{0}'", p.ToString());
                AddToList(p);
            }
        }

        private void AddToList(IPoint p)
        {
            ListViewItem i = new ListViewItem(p.ToString());
            i.Tag = p;
            listViewPoints.Items.Add(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x, y;
            Random r = new Random(DateTime.Now.Millisecond);
            x = r.Next(0, 99);
            y = r.Next(0, 99);

            logger.Log("Creating a random point @ ({0}, {1})", x, y);
            IPoint p = points.Create(x, y);
            points.Add(p);
            AddToList(p);
        }

        private void listViewPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = listViewPoints.SelectedItems != null && listViewPoints.SelectedItems.Count > 0;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewPoints.SelectedItems)
            {
                IPoint p = i.Tag as IPoint;
                logger.Log("Removing '{0}'", p.ToString());
                points.Remove(p);
                listViewPoints.Items.Remove(i);
            }
        }
    }
}
