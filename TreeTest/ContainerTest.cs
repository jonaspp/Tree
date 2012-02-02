using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest.Impl;
using Tree.Test;
using Tree.Injector;
using Tree.Factory;
using Tree.Container;
using Tree.Grafeas;
using Tree.Grafeas.Impl;
using Tree.Configuration;
using System.Configuration;
using Tree;

namespace TreeTest
{
    [TestClass]
    public class ContainerTest : TreeTestCase
    {
        [Inject()]
        private ITest testObj;

        [TestMethod]
        public void TestInitialization()
        {
            Assert.IsTrue(Core.Container.Objects.ContainsValue(testObj));
            Assert.IsNotNull(testObj);

            Assert.AreEqual("Started", testObj.Test());
            Core.Container.Stop();
            Assert.AreEqual("Stopped", testObj.Test());
        }
    }
}
