using System.Runtime.InteropServices;

namespace TodoListCSharp.utils {
    public class Notification {
        [DllImport("user32.dll")]
        public static extern int MessageBeep(uint uType);

        public static void Warning() {
            MessageBeep(0x00000030);
        }
    }
}