using System;
using System.Collections.Generic;
using System.Text;
using Bamboo.Prevalence;

namespace Tree.Archeio.ObjectStore.Impl.Commands
{
    [Serializable]
    public class RemoveFromCacheCommand<T> : ICommand where T : PersistentObject
    {
        private T obj;
        public RemoveFromCacheCommand(T obj)
        {
            this.obj = obj;
        }
        public object Execute(object system)
        {
            PrevalentCache cache = (PrevalentCache)system;
            cache.Remove(typeof(T), obj);
            return cache;
        }
    }
}
