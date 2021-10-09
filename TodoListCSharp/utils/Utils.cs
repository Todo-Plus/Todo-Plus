using System;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace TodoListCSharp.utils {
    public class Utils {
        // !! dll import
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        public static IntPtr SearchDesktopHandle() {
            IntPtr hRoot = GetDesktopWindow();
            IntPtr hShellDll = IntPtr.Zero;
            IntPtr hDesktop = FindWindowEx(hRoot, IntPtr.Zero, "WorkerW", String.Empty);
            while (true) {
                hShellDll = FindWindowEx(hDesktop, IntPtr.Zero, "SHELLDLL_DefView", String.Empty);
                if (hShellDll != IntPtr.Zero) {
                    return hDesktop;
                }
                hDesktop = FindWindowEx(hRoot, hDesktop, "WorkerW", String.Empty);
            }

            return IntPtr.Zero;
        }

        public static string MediaColorToHex(Color color) {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}