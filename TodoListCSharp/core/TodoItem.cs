using System;

namespace TodoListCSharp.core {
    [Serializable]
    public class TodoItem {
        private int Index { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        private bool Done { get; set; }
        private int Tag { get; set; }
        private int prority { get; set; }
        private DateTime Deadline { get; set; }
        private DateTime Starttime { get; set; }
        private DateTime Endtime { get; set; }
        private DateTime Donetime { get; set; }

        public TodoItem(string title, string desc) {
            Title = title;
            Desc = desc;
        }

        public void SetIndex(int index) {
            Index = index;
        }
    }
}