using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Tree.Archeio.ObjectStore
{
    [Serializable]
    public class PrevalentCache : MarshalByRefObject
    {
        private Dictionary<Type, List<PersistentObject>> cache = new Dictionary<Type, List<PersistentObject>>();

        public long Size()
        {
            long count = 0;
            foreach (Type t in cache.Keys)
            {
                count += cache[t].Count;
            }
            return count;
        }

        public void Clear()
        {
            cache.Clear();
        }

        public List<PersistentObject> Get(Type t)
        {
            List<PersistentObject> result = new List<PersistentObject>();

            if (!cache.ContainsKey(t))
            {
                cache.Add(t, new List<PersistentObject>());
            }
            return cache[t];
        }

        public void Put(Type t, List<PersistentObject> objects)
        {
            cache.Add(t, objects);
        }

        public void Remove(PersistentObject obj)
        {
            List<PersistentObject> all = Get(obj.GetType());
            long id = obj.Id;
            if (all.Count > 0)
            {
                foreach (PersistentObject o in all)
                {
                    if (o.Id == obj.Id)
                    {
                        all.Remove(obj);
                        break;
                    }
                }
            }
        }

        public void Add(PersistentObject obj)
        {
            Type t = obj.GetType();
            List<PersistentObject> objects = Get(t);
            objects.Add(obj);
        }

        public override string ToString()
        {
            return "PrevalentCache (" + Size() + ")";
        }
    }
}
