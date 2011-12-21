using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDemo.Points
{
    interface IPointManager
    {
        List<IPoint> Points();
        void Add(IPoint p);
        void Remove(IPoint p);
        IPoint Create(int x, int y);
    }
}
