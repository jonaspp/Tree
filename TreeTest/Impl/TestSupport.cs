using System;
using System.Collections.Generic;
using System.Text;
using Tree.Factory;
using Tree.Injector;
using Tree.Log;
using Tree.Lifecycle;
using System.Configuration;
using Tree.Configuration;

namespace TreeTest.Impl
{
    public class TestSupport : ITest, IConfigure
    {
        private string val;

        public string Test()
        {
            return val;
        }

        public void Configure(ContainerElement element)
        {
            val = element.Impl;
        }
    }
}
