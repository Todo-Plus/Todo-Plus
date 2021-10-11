using System;
using System.Collections.Generic;

// 用于整合列表进行保存，不用分文件保存
// 待实现

namespace TodoListCSharp.core {
    
    [Serializable]
    public class Save {
        public List<TodoItem> todolist;
        public List<TodoItem> donelist;
    }
}