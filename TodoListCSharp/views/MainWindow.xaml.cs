﻿using System.Windows;
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
using MessageBox = TodoListCSharp.views.MessageBox;

namespace TodoListCSharp {
    /// <summary>
    /// 程序主窗口
    /// </summary>
    public partial class MainWindow : Window {
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
        private List<Tab> tabs = new List<Tab>();
        private List<TodoItem> oShowTodoList = null;
        private Constants.MainWindowStatu statu = Constants.MainWindowStatu.TODO;
        private Constants.MainWindowLockStatu locked = Constants.MainWindowLockStatu.DRAGABLE;
        private int iMaxIndex = 0;
        private Setting setting = null;
        private int iSaveVersion = 0;

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

            Save save = null;
            IOInterface io = new BinaryIO();
            int ret = io.FileToSave(Constants.SAVE_FILEPATH, ref save);

            if (ret != 0) {
                throw new Exception();
            }

            oTodoItemList = Utils.ListToItemListForIO(save.todolist);
            oDoneItemList = Utils.ListToItemListForIO(save.donelist);
            tabs = save.tabs;
            iSaveVersion = save.version;

            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        public void MainWindow_onClosed(object sender, EventArgs e) {
            IOInterface io = new BinaryIO();
            Save save = new Save();
            save.todolist = oTodoItemList.GetItemListForSerializer();
            save.donelist = oDoneItemList.GetItemListForSerializer();
            save.tabs = tabs;
            save.version = iSaveVersion + 1;

            io.SaveToFile(ref save, Constants.SAVE_FILEPATH);
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
                locked = Constants.MainWindowLockStatu.LOCKED;
                IntPtr desktopHandle = Utils.SearchDesktopHandle();
                SetParent(hMainWindowHandle, desktopHandle);
                this.ApplicationMainWindow.ResizeMode = ResizeMode.NoResize;
                return;
            }
            locked = Constants.MainWindowLockStatu.DRAGABLE;
            this.ApplicationMainWindow.ResizeMode = ResizeMode.CanResize;
            SetParent(hMainWindowHandle, IntPtr.Zero);
        }

        /// <summary>
        /// 主窗口列表事件——事项完成
        /// </summary>
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
                oSettingWindow.ShowDialog();
                return;
            }

            oSettingWindow = new SettingWindow();
            oSettingWindow.setting = setting;
            oSettingWindow.tabs = tabs;
            oSettingWindow.settingWindowClosed += SettingWindow_OnClosed;
            oSettingWindow.AppearanceSettingChangeCallback += AppearanceSettingChange;
            oSettingWindow.TabAddCallback += TabAddEvent;
            oSettingWindow.Owner = this;
            // 算是一个坑，使用showdialog的之前一定要先初始化数据
            oSettingWindow.ShowDialog();
        }

        private void OpenItemAddWindow(object sender, RoutedEventArgs e) {
            if (oItemAddWindow != null) {
                oItemAddWindow.ShowDialog();
                return;
            }

            oItemAddWindow = new ItemAddWindow(tabs);

            // Set Callback Function
            oItemAddWindow.closeCallbackFunc += ItemAddWindow_onClosed;
            oItemAddWindow.AddItemToList += AddItemToList;
            oItemAddWindow.Owner = this;
            oItemAddWindow.ShowDialog();
        }

        // Set SettingWindow while close windows
        private void SettingWindow_OnClosed() {
            oSettingWindow = null;
            this.Activate();
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
            this.MainWindowBorder.Background = new SolidColorBrush(setting.BackgroundColor);
            this.MainWindowBorder.Background.Opacity = alpha;
        }

        public void TabAddEvent(Tab oNewTab) {
            tabs.Add(oNewTab);
        }
    }

    // !! subClass Define
}