using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoListCSharp.core;

namespace TodoPlusTest {
    [TestClass]
    public class ExampleUnitTest {
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
