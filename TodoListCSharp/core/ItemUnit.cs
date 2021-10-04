using TodoListCSharp.core;
namespace TodoListCSharp.core {
    public class ItemUnit {
        private TodoItem item { set; get; }
        private ItemUnit NextItem { set; get; }

        public int SetNext(ItemUnit unit) {
            NextItem = unit;
            return 0;
        }

        public ItemUnit GetNext() {
            return NextItem;
        }

        public TodoItem GetItem() {
            return item;
        }
        
        public ItemUnit(TodoItem item) {
            this.item = item;
            NextItem = null;
        }
    }
}