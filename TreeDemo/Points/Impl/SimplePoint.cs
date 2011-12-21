using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDemo.Points.Impl
{
    [Serializable]
    class SimplePoint : IPoint
    {
        public SimplePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public long Id
        {
            get;
            set;
        }
    }
}
