using System.Windows.Media;

namespace TodoListCSharp.core {
    public class Constants {
        public static string TODOITEM_FILEPATH = "./todolist";
        public static string DONEITEM_FILEPATH = "./donelist";
        public static string SETTING_FILEPATH = "./settings";
        public static string SAVE_FILEPATH = "./save";
        public static string TENCENT_OSS_PATH = "todoplus/save";

        public static Color MEDIA_COLOR_BALCK = Color.FromRgb(0, 0, 0);
        public static Color MEDIA_COLOR_WHITE = Color.FromRgb(255, 255, 255);

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

        public enum SettingStatu {
            MAIN,
            GENERAL,
            APPEARANCE,
            BACKUP,
            ABOUT
        }
    }
}