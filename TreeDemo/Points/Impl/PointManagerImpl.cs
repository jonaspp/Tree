using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;
using Tree.Injector;
using Tree.Archeio.ObjectStore;

namespace TreeDemo.Points.Impl
{
    public class PointManagerImpl : IPointManager, IInitializable
    {
        [Inject()]
        private ObjectStore store;

        private List<IPoint> points = new List<IPoint>();

        public void Initialize()
        {
            points = store.GetAll(typeof(IPoint)).ConvertAll<IPoint>
            (new Converter<PersistentObject, IPoint>
                (delegate(PersistentObject o)
                {
                    return (IPoint)o;
                }
             ));
        }

        #region IPointManager Members

        public List<IPoint> Points()
        {
            return points;
        }

        public void Add(IPoint p)
        {
            points.Add(p);
            store.Store(p);
            store.TakeSnapshot();
        }

        public void Remove(IPoint p)
        {
            points.Remove(p);
            store.Delete(p);
            store.TakeSnapshot();
        }

        public IPoint Create(int x, int y)
        {
            return new SimplePoint(x, y);
        }

        #endregion
    }
}
