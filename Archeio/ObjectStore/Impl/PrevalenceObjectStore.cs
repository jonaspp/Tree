using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Bamboo.Prevalence;
using Tree.Archeio.ObjectStore.Impl.Commands;
using Tree.Injector;

namespace Tree.Archeio.ObjectStore.Impl
{
    public class PrevalenceObjectStore : ObjectStore
    {
        private PrevalenceEngine engine = null;
        private PrevalentCache cache = null;
        private object syncRoot = new object();

        public PrevalenceObjectStore()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data");
            engine = PrevalenceActivator.CreateEngine(typeof(PrevalentCache), path);
            cache = engine.PrevalentSystem as PrevalentCache;
        }

        public PersistentObject GetById(Type t, long id)
        {
            List<PersistentObject> all = cache.Get(t);
            foreach (PersistentObject obj in all)
            {
                if (obj.Id == id)
                {
                    ObjectInjector.Inject(obj);
                    return obj;
                }
            }
            return null;
        }

        public List<PersistentObject> GetAll(Type t)
        {
            List<PersistentObject> all = cache.Get(t);
            foreach (PersistentObject obj in all)
            {
                ObjectInjector.Inject(obj);
            }
            return all;
        }

        public long Store(PersistentObject obj)
        {
            ICommand command;
            long nextId = 0;
            if (obj.Id <= 0)
            {
                nextId = NextId(obj);
                obj.Id = nextId;
                command = new AddToCacheCommand(obj);
            }
            else
            {
                command = new UpdateOnCacheCommand(obj);
            }
            engine.ExecuteCommand(command);
            return nextId;
        }

        public void Insert(PersistentObject obj)
        {
            Store(obj);
        }

        public long Update(PersistentObject obj)
        {
            return Store(obj);
        }

        public long Delete(PersistentObject obj)
        {
            RemoveFromCacheCommand command = new RemoveFromCacheCommand(obj);
            engine.ExecuteCommand(command);
            return obj.Id;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void TakeSnapshot()
        {
            engine.TakeSnapshot();
        }

        private long NextId(PersistentObject obj)
        {
		    lock (syncRoot)
            {
	            Type t = obj.GetType();
                List<PersistentObject> all = GetAll(t);
                if(all.Count == 0)
                {
                    return 1;
                }
                long nextId = 0;
                foreach (PersistentObject tmp in all)
                {
                    if(tmp.Id > nextId)
                    {
                        nextId = tmp.Id;
                    }
                }
                return nextId + 1; 
            }
        }
    }
}
