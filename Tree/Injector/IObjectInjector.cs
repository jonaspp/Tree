using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace Tree.Injector
{
    public interface IObjectInjector
    {
        void Inject(object obj);

        void Inject(object obj, Type objType);
    }
}
