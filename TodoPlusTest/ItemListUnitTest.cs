using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoListCSharp.core;

namespace TodoPlusTest {
    [TestClass]
    public class ItemListUnitTest {
        [TestMethod]
        public void AppendAndDeleteItemTest() {
            TodoItem todoItem1 = new TodoItem("test001", "test001");
            TodoItem todoItem2 = new TodoItem("test002", "test002");

            ItemList list = new ItemList();
            list.AppendItem(todoItem1);
            list.AppendItem(todoItem2);

            System.Collections.Generic.List<TodoItem> todoItems = list.GetItemList();
            Assert.AreEqual(todoItems.Count, 2);

            list.DeleteItem(0);
            todoItems = list.GetItemList();
            Assert.AreEqual(todoItems.Count, 1);
        }
    }
}
