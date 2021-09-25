using System.Windows;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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

            List<TodoItem> list = new List<TodoItem>();
            list.Add(new TodoItem() { 
                iIndex = 1,
                strTitle = "Test Item 1"
            });

            this.todoList.ItemsSource = list;
        }

        private void ButtonClickedLockWindow(object sender, RoutedEventArgs e) {

        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            base.DragMove();
        }
    }

    public class TodoItem {
        // todo： 修改为private并且设置对应的函数
        public int iIndex { set; get; }
        public string strTitle { set; get; }
        public string strDesc { set; get; }
        public bool bDone { set; get; }
    }
}
