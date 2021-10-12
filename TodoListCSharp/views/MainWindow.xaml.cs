using System.Windows;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TodoListCSharp.controls;
using TodoListCSharp.views;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;
using TodoListCSharp.utils;
using Color = System.Drawing.Color;

namespace TodoListCSharp
{
    /// <summary>
    /// 程序主窗口
    /// </summary>
    public partial class MainWindow : Window
    {
        // !! DllImport Define
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt64 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt64 dwNewLong);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        
        // !! Property Define

        private SettingWindow oSettingWindow = null;
        private ItemAddWindow oItemAddWindow = null;
        private ItemList oTodoItemList = null;
        private ItemList oDoneItemList = null;
        private List<TodoItem> oShowTodoList = null;
        private Constants.MainWindowStatu statu = Constants.MainWindowStatu.TODO;
        private Constants.MainWindowLockStatu locked = Constants.MainWindowLockStatu.DRAGABLE;
        private int iMaxIndex = 0;
        private Setting setting = null;
        
        private IntPtr hMainWindowHandle = IntPtr.Zero;
        public Visibility eDoneButtonStatu { get; set; }
        public Visibility eTodoButtonStatu { get; set; }

        // !! Functions Define and Implement
        public MainWindow() {
            InitializeComponent();
        }

        private void MainWindow_onLoaded(object sender, EventArgs e) {
            const int GWL_STYLE = (-16);
            const UInt64 WS_CHILD = 0x40000000;
            UInt64 iWindowStyle = GetWindowLong(hMainWindowHandle, GWL_STYLE);
            SetWindowLong(hMainWindowHandle, GWL_STYLE,
                (iWindowStyle | WS_CHILD));

            eTodoButtonStatu = Visibility.Visible;
            eDoneButtonStatu = Visibility.Collapsed;

            setting = new Setting();

            IOInterface io = new BinaryIO();
            int ret = io.FileToList(Constants.TODOITEM_FILEPATH, ref oTodoItemList);
            if (ret != 0) {
                // todo: 定义错误并进行提示
                throw new Exception();
            }

            ret = io.FileToList(Constants.DONEITEM_FILEPATH, ref oDoneItemList);
            if (ret != 0) {
                throw new Exception();
            }

            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        public void MainWindow_onClosed(object sender, EventArgs e) {
            BinaryIO io = new BinaryIO();
            io.ListToFile(ref oTodoItemList, Constants.TODOITEM_FILEPATH);
            io.ListToFile(ref oDoneItemList, Constants.DONEITEM_FILEPATH);
        }

        private void TodoButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.TODO) return;
            SwitchItemList();
        }

        private void DoneButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.DONE) return;
            SwitchItemList();
        }

        private void LockWindowButton_onClicked(object sender, RoutedEventArgs e) {
            hMainWindowHandle = new WindowInteropHelper(this).Handle;

            if (locked == Constants.MainWindowLockStatu.DRAGABLE) {
                IntPtr desktopHandle = Utils.SearchDesktopHandle();
                SetParent(hMainWindowHandle, desktopHandle);
                locked = Constants.MainWindowLockStatu.LOCKED;
                return;
            }

            SetParent(hMainWindowHandle, IntPtr.Zero);
            locked = Constants.MainWindowLockStatu.DRAGABLE;
        }

        /// <summary>
        /// 主窗口列表事件——事项完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemDoneButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton) sender;
            oTodoItemList.DoneOrRevertItem(button.Index, ref oDoneItemList);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        private void ItemRevertButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton) sender;
            oDoneItemList.DoneOrRevertItem(button.Index, ref oTodoItemList);
            oShowTodoList = oDoneItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        private void ItemDeleteButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton) sender;
            oTodoItemList.DeleteItem(button.Index);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        public void StickItemButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton) sender;
            oTodoItemList.SetItemToTop(button.Index);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (locked == Constants.MainWindowLockStatu.LOCKED) return;
            base.DragMove();
        }

        public void OpenSettingWindow(object sender, RoutedEventArgs e) {
            if (oSettingWindow != null) {
                oSettingWindow.Show();
                return;
            }

            oSettingWindow = new SettingWindow();
            oSettingWindow.Show();
            // Set callback function
            oSettingWindow.setting = setting;
            oSettingWindow.settingWindowClosed += SettingWindow_OnClosed;
            oSettingWindow.TransparencyChangeCallback += AppearanceSettingChange;
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
            AppearanceSettingChange(setting);
        }

        private void ItemAddWindow_onClosed() {
            oItemAddWindow = null;
        }

        private void AddItemToList(ref TodoItem item) {
            item.SetIndex(iMaxIndex);
            int ret = oTodoItemList.AppendItem(item);
            iMaxIndex++;
            oShowTodoList.Add(item);
            // 懒加载，每次添加直接扔到列表，顶置或者中间修改再进行直接的更新
            todoList.Items.Refresh();
        }

        private void SwitchItemList() {
            if (statu == Constants.MainWindowStatu.TODO) {
                oShowTodoList = oDoneItemList.GetItemList();
                TodoLabel.Style = (Style) FindResource("MWTypeUnchosedFont");
                DoneLabel.Style = (Style) FindResource("MWTypeChosedFont");
                TodoLine.Style = (Style) FindResource("MWTypeUnderlineUnchosed");
                DoneLine.Style = (Style) FindResource("MWTypeUnderlineChosed");
                eTodoButtonStatu = Visibility.Collapsed;
                eDoneButtonStatu = Visibility.Visible;
                statu = Constants.MainWindowStatu.DONE;
                // 不会自动同步，需要手动重新绑定
                todoList.ItemsSource = oShowTodoList;
                todoList.Items.Refresh();
            }
            else {
                oShowTodoList = oTodoItemList.GetItemList();
                TodoLabel.Style = (Style) FindResource("MWTypeChosedFont");
                DoneLabel.Style = (Style) FindResource("MWTypeUnchosedFont");
                TodoLine.Style = (Style) FindResource("MWTypeUnderlineChosed");
                DoneLine.Style = (Style) FindResource("MWTypeUnderlineUnchosed");
                eTodoButtonStatu = Visibility.Visible;
                eDoneButtonStatu = Visibility.Collapsed;
                statu = Constants.MainWindowStatu.TODO;
                todoList.ItemsSource = oShowTodoList;
                todoList.Items.Refresh();
            }
        }
        
        // !! MainWindow Appearance Change Functions

        public void AppearanceTransparencyChange(int value) {
            double alpha = value / 100.0;

            this.ApplicationMainWindow.Background = new SolidColorBrush(Colors.White);
            this.ApplicationMainWindow.Background.Opacity = alpha;
        }

        public void AppearanceSettingChange(Setting setting) {
            double alpha = setting.Alpha / 100.0;
            this.WindowBorder.Background = new SolidColorBrush(setting.BackgroundColor);
            this.WindowBorder.Background.Opacity = alpha;
            
            
        }
    }
    
    // !! subClass Define
}
