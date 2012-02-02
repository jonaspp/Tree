using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Bamboo.Prevalence;
using Tree.Archeio.ObjectStore.Impl.Commands;
using Tree.Injector;
using Tree.Lifecycle;
using Tree.Configuration;

namespace Tree.Archeio.ObjectStore.Impl
{
    public class PrevalenceObjectStore : IObjectStore, IConfigurable
    {
        private bool snapshotAtStart;

        private string path;

        [Inject]
        private IObjectInjector injector;
        
        private PrevalenceEngine engine = null;
        private PrevalentCache cache = null;
        private object syncRoot = new object();

        public PrevalenceObjectStore()
        {
            engine = PrevalenceActivator.CreateEngine(typeof(PrevalentCache), path);
            cache = engine.PrevalentSystem as PrevalentCache;
            if (snapshotAtStart)
            {
                engine.TakeSnapshot();
            }
        }

        public PersistentObject GetById(Type t, long id)
        {
            List<PersistentObject> all = cache.Get(t);
            foreach (PersistentObject obj in all)
            {
                if (obj.Id == id)
                {
                    injector.Inject(obj);
                    return obj;
                }
            }
            return null;
        }

        public T GetById<T>(long id) where T : PersistentObject
        {
            List<T> all = GetAll<T>();
            foreach (T obj in all)
            {
                if (obj.Id == id)
                {
                    return obj;
                }
            }
            return default(T);
        }

        public List<T> GetAll<T>() where T : PersistentObject
        {
            List<T> all = cache.Get<T>();
            foreach (T obj in all)
            {
                injector.Inject(obj);
            }
            return all;
        }

        public List<PersistentObject> GetAll(Type t)
        {
            List<PersistentObject> all = cache.Get(t);
            foreach (PersistentObject obj in all)
            {
                injector.Inject(obj);
            }
            return all;
        }

        public long Delete<T>(T obj) where T : PersistentObject
        {
            RemoveFromCacheCommand<T> command = new RemoveFromCacheCommand<T>(obj);
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

        private long NextId<T>(T obj) where T: PersistentObject
        {
		    lock (syncRoot)
            {
                Type t = typeof(T);
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

        public long Store<T>(T obj) where T : PersistentObject
        {
            ICommand command;
            long nextId = 0;            
            if (obj.Id <= 0)
            {
                nextId = NextId(obj);
                obj.Id = nextId;
                command = new AddToCacheCommand<T>(obj);
            }
            else
            {
                command = new UpdateOnCacheCommand<T>(obj);
            }
            engine.ExecuteCommand(command);
            return nextId;
        }


        public void Configure()
        {
        }
    }
}
