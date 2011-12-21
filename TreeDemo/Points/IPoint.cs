using System;
using System.Collections.Generic;
using System.Text;
using Tree.Archeio.ObjectStore;

namespace TreeDemo.Points
{
    public interface IPoint : PersistentObject
    {
        int X { get; }
        int Y { get; }
    }
}
