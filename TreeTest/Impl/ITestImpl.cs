using System;
using System.Collections.Generic;
using System.Text;
using Tree.Factory;

namespace TreeTest.Impl
{
    public class ITestImpl : ITest
    {
        [Inject()]
        private IWork worker;

        public string Test()
        {
            return worker.Get();
        }
    }
}
