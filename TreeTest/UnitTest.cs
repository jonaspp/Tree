using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest.Impl;
using Tree.Test;
using Tree.Injector;
using Tree.Factory;
using Tree.Container;
using System.Configuration;

namespace TreeTest
{
    [TestClass]
    public class UnitTest : TreeTestCase
    {
        [Inject()]
        private ITest testObj;

        [TestMethod]
        public void TestObjectContainer()
        {
            Assert.IsTrue(ObjectContainer.Static.Objects.ContainsValue(testObj));
            Assert.IsNotNull(testObj);
            ITest anotherObj = ObjectContainer.Lookup<ITest>();
            Assert.AreSame(testObj, anotherObj);
        }

        [TestMethod]
        public void TestContext()
        {
            ITest anotherObj = ObjectFactory.Create<TestImpl>();
            Assert.IsNotNull(anotherObj);
            Assert.AreNotSame(testObj, anotherObj);
        }

        [TestMethod]
        public void TestLookup()
        {
            Type t = ObjectFactory.ImplFor(typeof(ITest));
            Assert.AreEqual(t, typeof(TestImpl));
        }

        [TestMethod]
        public void TestObjectConfiguration()
        {
            Assert.AreEqual(testObj.Test(), "TreeTest.Impl.TestSupport");
        }
    }
}
