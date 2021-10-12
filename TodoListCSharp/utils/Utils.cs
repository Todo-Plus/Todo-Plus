using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Runtime.InteropServices;
using TodoListCSharp.core;

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

        public static Color HexToMediaColor(string sColorString) {
            return (Color)ColorConverter.ConvertFromString(sColorString);
        }

        public static int TabListGetPosition(ref List<Tab> tabs, int iIndex) {
            int length = tabs.Count;
            for (int i = 0; i < length; i++) {
                if (tabs[i].Id == iIndex) {
                    return i;
                }
            }

            return -1;
        }
    }
}