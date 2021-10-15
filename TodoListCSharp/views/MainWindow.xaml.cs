using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using TodoListCSharp.controls;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;
using TodoListCSharp.utils;
using TodoListCSharp.views;

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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // !! Property Define

        private SettingWindow oSettingWindow = null;
        private ItemAddWindow oItemAddWindow = null;
        private ItemList oTodoItemList = null;
        private ItemList oDoneItemList = null;
        private List<Tab> tabs = new List<Tab>();
        private List<TodoItem> oShowTodoList = null;
        private Constants.MainWindowStatu statu = Constants.MainWindowStatu.TODO;
        private Constants.MainWindowLockStatu locked = Constants.MainWindowLockStatu.DRAGABLE;

        private static readonly string _regPath = @"Software\TodoPlus\";

        private int iMaxIndex = 0;
        private Setting setting = null;
        private int iSaveVersion = 0;

        private IntPtr hMainWindowHandle = IntPtr.Zero;
        public Visibility eDoneButtonStatu { get; set; }
        public Visibility eTodoButtonStatu { get; set; }

        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource _HwndSource;

        // !! Functions Define and Implement
        public MainWindow() {
            InitializeComponent();

            this.SourceInitialized += delegate (object sender, EventArgs e) {
                this._HwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            };
        }

        private void MainWindow_onLoaded(object sender, EventArgs e) {
            const int GWL_STYLE = (-16);
            const UInt64 WS_CHILD = 0x40000000;
            UInt64 iWindowStyle = GetWindowLong(hMainWindowHandle, GWL_STYLE);
            SetWindowLong(hMainWindowHandle, GWL_STYLE,
                (iWindowStyle | WS_CHILD));

            eTodoButtonStatu = Visibility.Visible;
            eDoneButtonStatu = Visibility.Collapsed;

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

            setting = new Setting();
            setting.ReadSettingFromRegistryTable();
            MainSetSize(this, setting);
            MainWindowAppearanceLoadSetting(setting);
        }

        private void MainWindow_onClosing(object sender, EventArgs e) {
            setting.SaveSettingToRegistryTable(this);
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

        private void MainWindow_onResize(object sender, EventArgs e) {
            this.todoList.Height = this.Height - 85;
        }

        public void MainSetSize(Window window, Setting setting) {
            if (setting.WindowBounds == Rect.Empty) {
                return;
            }

            var WindowBounds = setting.WindowBounds;
            window.Top = WindowBounds.Top;
            window.Left = WindowBounds.Left;
            window.Width = WindowBounds.Width;
            window.Height = WindowBounds.Height;

            // todo：实现存在一定的问题，待修改
            // if (key != null) {
            //     locked = Enum.Parse<Constants.MainWindowLockStatu>(key.GetValue("LockStatus").ToString());
            //     // 默认为可抓取状态，转到锁定状态
            //     if (locked == Constants.MainWindowLockStatu.LOCKED) {
            //         SwitchWindowLockStatus();
            //     }
            // }
        }

        public void MainWindowAdaptBackgroundColor(System.Windows.Media.Color color) {
            System.Windows.Media.Color TextColor = Utils.GenerateAdaptColor(color);

            if (statu == Constants.MainWindowStatu.TODO) {
                this.TodoLabel.Foreground = new SolidColorBrush(TextColor);
                this.TodoLine.BorderBrush = new SolidColorBrush(TextColor);
            }
            else {
                this.DoneLabel.Foreground = new SolidColorBrush(TextColor);
                this.DoneLine.BorderBrush = new SolidColorBrush(TextColor);
            }

            int length = oShowTodoList.Count;
            for (int i = 0; i < length; i++) {
                oShowTodoList[i].ForgeColor = Utils.MediaColorToHex(TextColor);
            }
            todoList.Items.Refresh();
        }

        private void TodoButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.TODO) return;
            SwitchItemList();
        }

        private void DoneButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.DONE) return;
            SwitchItemList();
        }

        private void LockWindowButton_onClicked(object sender, RoutedEventArgs e) => SwitchWindowLockStatus();

        private void SwitchWindowLockStatus() {
            MainWindowAppearanceLoadSetting(setting);
            hMainWindowHandle = new WindowInteropHelper(this).Handle;

            if (locked == Constants.MainWindowLockStatu.DRAGABLE) {
                locked = Constants.MainWindowLockStatu.LOCKED;
                RightBottomResizeButton.Visibility = Visibility.Collapsed;
                IntPtr desktopHandle = Utils.SearchDesktopHandle();
                SetParent(hMainWindowHandle, desktopHandle);
                return;
            }
            locked = Constants.MainWindowLockStatu.DRAGABLE;
            RightBottomResizeButton.Visibility = Visibility.Visible;
            SetParent(hMainWindowHandle, IntPtr.Zero);
        }

        /// <summary>
        /// 主窗口列表事件——事项完成
        /// </summary>
        private void ItemDoneButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oTodoItemList.DoneOrRevertItem(button.Index, ref oDoneItemList);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        private void ItemRevertButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oDoneItemList.DoneOrRevertItem(button.Index, ref oTodoItemList);
            oShowTodoList = oDoneItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        private void ItemDeleteButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oTodoItemList.DeleteItem(button.Index);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
        }

        public void StickItemButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
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
            // 备份一份设置扔给设置窗口
            oSettingWindow.setting = new Setting(setting);
            oSettingWindow.tabs = tabs;
            oSettingWindow.settingWindowClosed += SettingWindow_OnClosed;
            oSettingWindow.AppearanceSettingChangeCallback += MainWindowAppearanceLoadSetting;
            oSettingWindow.RollbackSettingCallback += MainWindowRollbackSetting;
            oSettingWindow.SettingConfirmCallback += MainWindowConfirmSetSetting;
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
            MainWindowAppearanceLoadSetting(setting);
            this.Activate();
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
                TodoLabel.Style = (Style)FindResource("MWTypeUnchosedFont");
                DoneLabel.Style = (Style)FindResource("MWTypeChosedFont");
                TodoLine.Style = (Style)FindResource("MWTypeUnderlineUnchosed");
                DoneLine.Style = (Style)FindResource("MWTypeUnderlineChosed");
                eTodoButtonStatu = Visibility.Collapsed;
                eDoneButtonStatu = Visibility.Visible;
                statu = Constants.MainWindowStatu.DONE;
                // 不会自动同步，需要手动重新绑定
                todoList.ItemsSource = oShowTodoList;
                todoList.Items.Refresh();
            }
            else {
                oShowTodoList = oTodoItemList.GetItemList();
                TodoLabel.Style = (Style)FindResource("MWTypeChosedFont");
                DoneLabel.Style = (Style)FindResource("MWTypeUnchosedFont");
                TodoLine.Style = (Style)FindResource("MWTypeUnderlineChosed");
                DoneLine.Style = (Style)FindResource("MWTypeUnderlineUnchosed");
                eTodoButtonStatu = Visibility.Visible;
                eDoneButtonStatu = Visibility.Collapsed;
                statu = Constants.MainWindowStatu.TODO;
                todoList.ItemsSource = oShowTodoList;
                todoList.Items.Refresh();
            }
        }

        // !! MainWindow Appearance Change Functions
        private void MainWindowAppearanceLoadSetting(Setting setting) {
            double alpha = setting.Alpha / 100.0;
            MainWindowAdaptBackgroundColor(setting.BackgroundColor);
            this.MainWindowBorder.Background = new SolidColorBrush(setting.BackgroundColor);
            this.MainWindowBorder.Background.Opacity = alpha;
        }

        private void MainWindowConfirmSetSetting(Setting oNewSetting) {
            setting = oNewSetting;
            MainWindowAppearanceLoadSetting(setting);
        }

        private void MainWindowRollbackSetting() {
            MainWindowAppearanceLoadSetting(setting);
        }

        public void TabAddEvent(Tab oNewTab) {
            tabs.Add(oNewTab);
        }

        private void ResizePressed(object sender, MouseEventArgs e) {
            FrameworkElement element = sender as FrameworkElement;
            this.Cursor = Cursors.Arrow;

            if (locked == Constants.MainWindowLockStatu.DRAGABLE && e.LeftButton == MouseButtonState.Pressed) {
                this.Cursor = Cursors.SizeNWSE;
                ResizeWindow();
            }
        }

        private void ResizeWindow() {
            SendMessage(_HwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + 8), IntPtr.Zero);
        }

        public bool ShowTipMessage() {
            return setting.CloseTips;
        }
    }

    // !! subClass Define
}