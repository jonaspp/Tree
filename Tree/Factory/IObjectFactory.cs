using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace Tree.Factory
{
    public interface IObjectFactory
    {
        T Create<T>(params object[] parameters) where T : class;

        Type ImplFor(Type type);

        object Create(Type type, params object[] parameters);

        Type TypeFrom(string type);

        Type TypeFromFullname(string type, string name);
    }
}
