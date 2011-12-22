using System;
using System.Collections.Generic;
using System.Text;
using Bamboo.Prevalence;

namespace Tree.Archeio.ObjectStore.Impl.Commands
{
    [Serializable]
    public class AddToCacheCommand<T> : ICommand where T : PersistentObject
    {
        private T obj;
        public AddToCacheCommand(T obj)
        {
            this.obj = obj;
        }
        public object Execute(object system)
        {
            PrevalentCache cache = (PrevalentCache)system;
            cache.Add(typeof(T), obj);
            return cache;
        }
    }
}
