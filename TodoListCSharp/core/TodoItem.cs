using System;

namespace TodoListCSharp.core {
    [Serializable]
    public class TodoItem {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ForgeColor { get; set; }
        public string PointColor { get; set; }
        private Constants.TodoItemStatu Statu { get; set; }
        public int Tag { get; set; }
        private int prority { get; set; }
        private DateTime Deadline { get; set; }
        private DateTime Starttime { get; set; }
        private DateTime Endtime { get; set; }
        private DateTime Donetime { get; set; }

        public TodoItem(string title, string desc) {
            Title = title;
            Desc = desc;
            PointColor = "#BBBBBB";
            ForgeColor = "#FFFFFF";
        }

        public void SetIndex(int index) {
            Index = index;
        }
    }
}