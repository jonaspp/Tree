using System;
using System.Collections.Generic;
using System.Text;
using Bamboo.Prevalence;

namespace Tree.Archeio.ObjectStore.Impl.Commands
{
    [Serializable]
    public class RemoveFromCacheCommand : ICommand
    {
        private PersistentObject obj = null;
        public RemoveFromCacheCommand(PersistentObject obj)
        {
            this.obj = obj;
        }
        public object Execute(object system)
        {
            PrevalentCache cache = (PrevalentCache)system;
            cache.Remove(obj);
            return cache;
        }
    }
}
