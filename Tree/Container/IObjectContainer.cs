using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace Tree.Container
{
    public interface IObjectContainer : IStartable
    {
        Dictionary<string, object> Objects { get; }
        
        T Get<T>() where T : class;

        T Register<T, C>(params object[] parameters)
            where T : class
            where C : T;

        T Lookup<T>(params object[] parameters) where T : class;

        object Lookup(Type t, params object[] parameters);
    }
}
