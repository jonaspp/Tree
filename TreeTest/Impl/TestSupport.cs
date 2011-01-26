using System;
using System.Collections.Generic;
using System.Text;
using Tree.Factory;
using Tree.Injector;
using Tree.Log;

namespace TreeTest.Impl
{
    public class TestSupport : ITest
    {
        [Inject()]
        private IWork worker;

        public string Test()
        {
            return worker.Get() + "support";
        }
    }
}
