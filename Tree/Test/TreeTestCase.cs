using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Tree.Injector;
using Tree.Container;

namespace Tree.Test
{
    public class TreeTestCase
    {
        public TreeTestCase()
        {
            Core.Injector.Inject(this);
        }
    }
}
