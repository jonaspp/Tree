using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Archeio.ObjectStore
{
    public interface ObjectStore
    {
        PersistentObject GetById(Type t, long id);

        List<PersistentObject> GetAll(Type t);

        long Store(PersistentObject obj);

        void Insert(PersistentObject obj);

        long Update(PersistentObject obj);

        long Delete(PersistentObject obj);

        void Clear();

        void TakeSnapshot();
    }
}
