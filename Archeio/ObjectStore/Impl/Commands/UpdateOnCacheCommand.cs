using System;
using System.Collections.Generic;
using System.Text;
using Bamboo.Prevalence;

namespace Tree.Archeio.ObjectStore.Impl.Commands
{
    [Serializable]
    public class UpdateOnCacheCommand : ICommand
    {
        private PersistentObject obj = null;
        public UpdateOnCacheCommand(PersistentObject obj)
        {
            this.obj = obj;
        }
        public object Execute(object system)
        {
            PrevalentCache cache = (PrevalentCache)system;
            cache.Remove(obj); 
            cache.Add(obj);
            return cache;
        }
    }
}
