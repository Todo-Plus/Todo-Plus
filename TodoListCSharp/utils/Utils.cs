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

        public static Color SliderPercentToColor(double percent) {
            Color oRetColor;
            double fRealPercent;
            const double total = 0.167;
            if (percent < 0.167) {
                fRealPercent = percent / total;
                int Green = (int)(255 * fRealPercent);
                oRetColor = Color.FromRgb(0xFF, (byte)Green, 0x00);
            }
            else if (percent < 0.333) {
                fRealPercent = (percent - 0.167) / total;
                int Red = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb((byte)Red, 0xFF, 0x00);
            }
            else if (percent < 0.5) {
                fRealPercent = (percent - 0.333) / total;
                int Blue = (int) (fRealPercent * 255);
                oRetColor = Color.FromRgb(0x00, 0xFF, (byte)Blue);
            }
            else if (percent < 0.667) {
                fRealPercent = (percent - 0.5) / total;
                int Green = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb(0x00, (byte)Green, 0xFF);
            }
            else if (percent < 0.833) {
                fRealPercent = (percent - 0.667) / total;
                int Red = (int) (fRealPercent * 255);
                oRetColor = Color.FromRgb((byte)Red, 0x00, 0xFF);
            }
            else {
                fRealPercent = (percent - 0.833) / total;
                int Blue = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb(0xFF, 0x00, (byte)Blue);
            }

            return oRetColor;
        }

        public static string MediaColorToHex(Color color) {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}