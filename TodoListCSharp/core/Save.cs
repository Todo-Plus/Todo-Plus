using System;
using System.Collections.Generic;

// 用于整合列表进行保存，不用分文件保存
// 待实现

namespace TodoListCSharp.core {

    [Serializable]
    public class Save {
        public int version;
        public List<TodoItem> todolist;
        public List<TodoItem> donelist;
        public List<Tab> tabs;

        public Save() {
            version = 0;
            todolist = new List<TodoItem>();
            donelist = new List<TodoItem>();
            tabs = new List<Tab>();
        }
    }
}