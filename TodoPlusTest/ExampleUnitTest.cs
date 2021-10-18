using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListCSharp.core;

namespace TodoPlusTest {
    [TestClass]
    class ExampleUnitTest {
        [TestMethod]
        public void AddFuncTest() {
            Example example = new Example();

            int DstResult = 900;
            int TestA = 348;
            int TestB = 552;

            Assert.AreEqual(example.add(TestA, TestB), DstResult);
        }
    }
}
