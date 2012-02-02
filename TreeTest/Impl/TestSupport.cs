using System;
using System.Collections.Generic;
using System.Text;
using Tree.Factory;
using Tree.Injector;
using Tree.Grafeas;
using Tree.Lifecycle;
using System.Configuration;
using Tree.Configuration;

namespace TreeTest.Impl
{
    public class TestSupport : ITest, Tree.Lifecycle.IConfigurable
    {
        private string val;

        public string Test()
        {
            return val;
        }

        public void Configure()
        {
        }
    }
}
