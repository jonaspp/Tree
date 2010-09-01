using System;
using System.Collections.Generic;
using System.Text;
using Tree.Factory;
using Tree.Injector;

namespace TreeTest.Impl
{
    public class TestImpl : ITest
    {
        [Inject()]
        private IWork worker;

        public string Test()
        {
            return worker.Get();
        }
    }
}
