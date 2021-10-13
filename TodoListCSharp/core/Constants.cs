namespace TodoListCSharp.core {
    public class Constants {
        public static string TODOITEM_FILEPATH = "./todolist";
        public static string DONEITEM_FILEPATH = "./donelist";
        public static string SETTING_FILEPATH = "./settings";
        public static string SAVE_FILEPATH = "./save";
        
        public enum MainWindowStatu {
            TODO,
            DONE
        }
        
        public enum MainWindowLockStatu {
            DRAGABLE,
            LOCKED
        }
        
        public enum TodoItemStatu {
            TODO,
            DONE
        }

        public enum SettingBackStatus {
            CONFIRM,
            CANCEL
        }
    }
}