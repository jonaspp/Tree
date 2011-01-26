using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest.Impl;
using Tree.Test;
using Tree.Injector;
using Tree.Factory;
using Tree.Container;
using Tree.Log;
using Tree.Log.Impl;
using Tree.Configuration;
using System.Configuration;

namespace TreeTest
{
    [TestClass]
    public class UnitTest : TestCase
    {
        [Inject()]
        private ITest testObj;

        [TestMethod]
        public void TestFactory()
        {
            Assert.IsTrue(ObjectContainer.StaticInstance.Objects.ContainsValue(testObj));

            Assert.IsNotNull(testObj);
            Assert.AreEqual("Idle", testObj.Test());

            ObjectContainer.StaticInstance.Start();
            Assert.AreEqual("Started", testObj.Test());

            ObjectContainer.StaticInstance.Stop();
            Assert.AreEqual("Stopped", testObj.Test());
        }
    }
}
