using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Archeio.ObjectStore
{
    public interface IObjectStore
    {
        T GetById<T>(long id) where T : PersistentObject;

        List<T> GetAll<T>() where T : PersistentObject;

        long Store<T>(T obj) where T : PersistentObject;

        long Delete<T>(T obj) where T : PersistentObject;

        void Clear();

        void TakeSnapshot();
    }
}
