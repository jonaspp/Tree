using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Injector;

namespace Tree.Test
{
    public class TestCase
    {
        public TestCase()
        {

            ObjectInjector.Inject(this);
        }
    }
}
