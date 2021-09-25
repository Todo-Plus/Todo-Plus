using System.Windows;
using System.IO;
using System.Runtime.InteropServices;

namespace TodoListCSharp
{
    /// <summary>
    /// 程序主窗口
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern System.IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32.dll")]
        public static extern System.IntPtr SetParent(System.IntPtr hWndChild, System.IntPtr hWndNewParent);
        public MainWindow() {
            InitializeComponent();
        }

        private void ButtonClickedLockWindow(object sender, RoutedEventArgs e) {

        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            base.DragMove();
        }
    }
}
