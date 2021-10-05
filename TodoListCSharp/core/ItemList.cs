using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoListCSharp.core;
namespace TodoListCSharp.core {
    public class ItemList {
        private ItemUnit ListStart;
        private ItemUnit ListEnd;

        public ItemList(TodoItem item) {
            ItemUnit newItem = new ItemUnit(item);
            ListStart = newItem;
        }
        
        public void SetItemToTop(ref ItemUnit ChosedItem) {
            return ;
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

        public int DoneItem(int iItemIndex, ref ItemList oDoneList) {
            ItemUnit oNowItem = ListStart;
            ItemUnit oBeforeItem = null;
            while (oNowItem != null) {
                if (oNowItem.GetItem().Index == iItemIndex) {
                    TodoItem item = oNowItem.GetItem();
                    // todo: switch item`s statu
                    if (oBeforeItem == null) {
                        ListStart = oNowItem.GetNext();
                        oDoneList.AppendItem(item);
                        return 0;
                    }

                    oBeforeItem.SetNext(oNowItem.GetNext());
                    oDoneList.AppendItem(item);
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

        public void SetListStart(ItemUnit start) {
            ListStart = start;
        }
    }
}