using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
            Assert.AreEqual<string>(todoItems[0].Title, todoItem1.Title);
            Assert.AreEqual<string>(todoItems[1].Title, todoItem2.Title);

            list.DeleteItem(0);
            todoItems = list.GetItemList();
            Assert.AreEqual<string>(todoItems[0].Title, todoItem2.Title); ;
        }

        [TestMethod]
        public void NewItemListTest() {
            TodoItem todoItem1 = new TodoItem("test001", "test001");
            ItemList todolist = new ItemList(todoItem1);
            List<TodoItem> TodoItemList = todolist.GetItemList();

            Assert.AreEqual(1, TodoItemList.Count);
        }

        [TestMethod]
        public void SetItemToTopTest() {
            ItemList todolist = new ItemList();

            TodoItem todoItem1 = new TodoItem("test001", "test001");
            TodoItem todoItem2 = new TodoItem("test002", "test002");
            todoItem1.SetIndex(0);
            todoItem2.SetIndex(1);
            todolist.AppendItem(todoItem1);
            todolist.AppendItem(todoItem2);
            List<TodoItem> TodoItemList = todolist.GetItemList();

            Assert.AreEqual(2, TodoItemList.Count);
            Assert.AreEqual<string>(todoItem1.Title, TodoItemList[0].Title);

            todolist.SetItemToTop(1);
            TodoItemList = todolist.GetItemList();
            Assert.AreEqual(2, TodoItemList.Count);
            Assert.AreEqual<string>(todoItem2.Title, TodoItemList[0].Title);
        }

        [TestMethod]
        public void DoneItemTest() {
            ItemList todolist = new ItemList();
            ItemList donelist = new ItemList();

            TodoItem todoItem1 = new TodoItem("test001", "test001");
            TodoItem todoItem2 = new TodoItem("test002", "test002");
            TodoItem todoItem3 = new TodoItem("test003", "test003");
            todoItem1.SetIndex(0);
            todoItem2.SetIndex(1);
            todoItem3.SetIndex(2);

            todolist.AppendItem(todoItem1);
            todolist.AppendItem(todoItem2);
            todolist.AppendItem(todoItem3);
            todolist.DoneOrRevertItem(0, ref donelist);
            todolist.DoneOrRevertItem(2, ref donelist);

            List<TodoItem> TodoItemList = todolist.GetItemList();
            List<TodoItem> DoneItemList = donelist.GetItemList();
            Assert.AreEqual(1, TodoItemList.Count);
            Assert.AreEqual(2, DoneItemList.Count);
            Assert.AreEqual<string>(todoItem2.Title, TodoItemList[0].Title);
            Assert.AreEqual<string>(todoItem1.Title, DoneItemList[0].Title);
            Assert.AreEqual<string>(todoItem3.Title, DoneItemList[1].Title);

            donelist.DoneOrRevertItem(0, ref todolist);
            TodoItemList = todolist.GetItemList();
            DoneItemList = donelist.GetItemList();
            Assert.AreEqual(1, DoneItemList.Count);
            Assert.AreEqual(2, TodoItemList.Count);
            Assert.AreEqual<string>(todoItem2.Title, TodoItemList[0].Title);
            Assert.AreEqual<string>(todoItem1.Title, TodoItemList[1].Title);
            Assert.AreEqual<string>(todoItem3.Title, DoneItemList[0].Title);
        }
    }
}
