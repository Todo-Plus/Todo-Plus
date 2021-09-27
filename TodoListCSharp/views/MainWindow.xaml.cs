using System.Windows;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using TodoListCSharp.views;

namespace TodoListCSharp
{
    /// <summary>
    /// 程序主窗口
    /// </summary>
    public partial class MainWindow : Window
    {
        // DllImport Define

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern System.IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32.dll")]
        public static extern System.IntPtr SetParent(System.IntPtr hWndChild, System.IntPtr hWndNewParent);

        // Property Define

        private SettingWindow oSettingWindow = null;

        // Functions Define and Implement
        public MainWindow() {
            InitializeComponent();

            List<TodoItem> list = new List<TodoItem>();
            

            for (int i = 0; i <= 40; i++) {
                list.Add(new TodoItem() {
                    iIndex = i,
                    strTitle = "Test item"
                });
            }

            this.todoList.ItemsSource = list;
        }

        private void ButtonClickedLockWindow(object sender, RoutedEventArgs e) {

        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            base.DragMove();
        }

        private void OpenSettingWindow(object sender, RoutedEventArgs e) {
            if (oSettingWindow != null) {
                oSettingWindow.Show();
                return;
            }

            oSettingWindow = new SettingWindow();
            oSettingWindow.Show();
            oSettingWindow.Owner = this;
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
