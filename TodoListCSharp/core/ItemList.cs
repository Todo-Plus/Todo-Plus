using System;
using System.Collections.Generic;
namespace TodoListCSharp.core {
    /// <summary>
    /// todo： 链表的实现还需要再优化一下，保证在链表为空的时候能够继续进行序列化
    /// </summary>
    public class ItemList {
        private ItemUnit ListStart;
        private ItemUnit ListEnd;

        public ItemList(TodoItem item) {
            ItemUnit newItem = new ItemUnit(item);
            ListStart = ListEnd = newItem;
        }

        // 返回链表转换为的列表，由于项目数量比较少，所以O(n)每一次转换足够
        public List<TodoItem> GetItemList() {
            List<TodoItem> oRetList = new List<TodoItem>();
            ItemUnit oNowItemUnit = ListStart;

            while (oNowItemUnit != null) {
                oRetList.Add(oNowItemUnit.GetItem());
                oNowItemUnit = oNowItemUnit.GetNext();
            }

            return oRetList;
        }

        public List<TodoItem> GetItemListForSerializer() {
            List<TodoItem> oRetList = GetItemList();
            TodoItem item = new TodoItem(String.Empty, String.Empty);
            item.SetIndex(-1);
            oRetList.Insert(0, item);
            return oRetList;
        }

        public ItemList() {

        }

        public int AppendItem(TodoItem item) {
            ItemUnit newItem = new ItemUnit(item);
            if (ListEnd != null) {
                ListEnd.SetNext(newItem);
                ListEnd = ListEnd.GetNext();
            }
            else ListEnd = ListStart = newItem;
            return 0;
        }

        /// <summary>
        /// 完成或回溯事项
        /// </summary>
        /// <param name="iItemIndex">事项编号</param>
        /// <param name="oDstList">放入的目标链表（todolist/donelist）</param>
        /// <returns></returns>
        public int DoneOrRevertItem(int iItemIndex, ref ItemList oDstList) {
            ItemUnit oNowItem = ListStart;
            ItemUnit oBeforeItem = null;
            while (oNowItem != null) {
                if (oNowItem.GetItem().Index == iItemIndex) {
                    TodoItem item = oNowItem.GetItem();
                    // todo: switch item`s statu
                    if (oBeforeItem == null) {
                        ListStart = oNowItem.GetNext();
                        // item list empty
                        if (ListStart == null) {
                            ListEnd = null;
                        }
                        oDstList.AppendItem(item);
                        return 0;
                    }

                    oBeforeItem.SetNext(oNowItem.GetNext());
                    if (oBeforeItem.GetNext() == null) ListEnd = oBeforeItem;
                    oDstList.AppendItem(item);
                    return 0;
                }

                oBeforeItem = oNowItem;
                oNowItem = oNowItem.GetNext();
            }

            return -1;
        }

        public int DeleteItem(int iItemIndex) {
            ItemUnit oNowItem = ListStart;
            ItemUnit oBeforeItem = null;
            while (oNowItem != null) {
                if (oNowItem.GetItem().Index == iItemIndex) {
                    TodoItem item = oNowItem.GetItem();
                    if (oBeforeItem == null) {
                        ListStart = oNowItem.GetNext();
                        return 0;
                    }
                    oBeforeItem.SetNext(oNowItem.GetNext());
                    return 0;
                }
                oBeforeItem = oNowItem;
                oNowItem = oNowItem.GetNext();
            }
            return -1;
        }

        public int SetItemToTop(int iItemIndex) {
            ItemUnit oNowItem = ListStart;
            ItemUnit oBeforeItem = null;
            while (oNowItem != null) {
                if (oNowItem.GetItem().Index == iItemIndex) {
                    TodoItem item = oNowItem.GetItem();
                    if (oBeforeItem == null) {
                        return 0;
                    }
                    oBeforeItem.SetNext(oNowItem.GetNext());
                    oNowItem.SetNext(ListStart);
                    ListStart = oNowItem;
                    return 0;
                }
                oBeforeItem = oNowItem;
                oNowItem = oNowItem.GetNext();
            }
            return -1;
        }
    }
}