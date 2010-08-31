using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tree.Factory;
using TreeTest.Impl;

namespace TreeTest
{
    [TestClass]
    public class UnitTest : TestCase
    {        
        public UnitTest()
        {
        }

        [TestMethod]
        public void TestFactory()
        {
            ITest testObj = ObjectFactory.Register<ITest, TestImpl>();
            Assert.IsNotNull(testObj);
            Assert.AreEqual("Job", testObj.Test());
        }
    }
}
