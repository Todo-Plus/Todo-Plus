using System.Windows;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using TodoListCSharp.views;
using TodoListCSharp.core;
using TodoListCSharp.utils;

namespace TodoListCSharp
{
    /// <summary>
    /// 程序主窗口
    /// </summary>
    public partial class MainWindow : Window
    {
        // !! DllImport Define

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern System.IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32.dll")]
        public static extern System.IntPtr SetParent(System.IntPtr hWndChild, System.IntPtr hWndNewParent);

        // !! Property Define

        private SettingWindow oSettingWindow = null;
        private ItemAddWindow oItemAddWindow = null;
        private ItemList oTodoItemList = null;
        private List<TodoItem> oShowTodoList = null;

        // !! Functions Define and Implement
        public MainWindow() {
            InitializeComponent();
        }

        private void MainWindow_onLoaded(object sender, EventArgs e) {
            BinaryIO io = new BinaryIO();
            int ret = io.FileToList(Constants.ITEM_FILEPATH, ref oTodoItemList);
            if (ret != 0) {
                // todo: 定义错误并进行提示
                throw new Exception();
            }

            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
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
            // Set callback function
            oSettingWindow.settingWindowClosed += SettingWindow_OnClosed;
            oSettingWindow.Owner = this;
        }

        private void OpenItemAddWindow(object sender, RoutedEventArgs e) {
            if (oItemAddWindow != null) {
                oItemAddWindow.Show();
                return;
            }

            oItemAddWindow = new ItemAddWindow();
            oItemAddWindow.Show();
            
            // Set Callback Function
            oItemAddWindow.closeCallbackFunc += ItemAddWindow_onClosed;
            oItemAddWindow.AddItemToList += AddItemToList;
            oItemAddWindow.Owner = this;
        }
        
        // Set SettingWindow while close windows
        private void SettingWindow_OnClosed() {
            oSettingWindow = null;
        }

        private void ItemAddWindow_onClosed() {
            oItemAddWindow = null;
        }

        private void AddItemToList(ref TodoItem item) {
            int ret = oTodoItemList.AppendItem(item);
            oShowTodoList.Add(item);
            // 懒加载，每次添加直接扔到列表，顶置或者中间修改再进行直接的更新
            todoList.Items.Refresh();
        }
    }
    
    // !! subClass Define
}
