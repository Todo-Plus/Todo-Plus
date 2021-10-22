using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using TodoListCSharp.controls;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;
using TodoListCSharp.threads;
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
        
        /// <summary>
        /// 声明一系列的对象：
        /// 窗口： 设置窗口及项目添加窗口
        /// 列表： 待办或已做事项的列表，以及用于展示的list，tab的列表
        /// 状态： 窗口当前处于的专题，选择的是待办还是已办，窗口是否锁定
        /// 常数： 当前最大的index值，用于标识一个item，存档的version值
        /// </summary>
        
        private SettingWindow oSettingWindow = null;
        private ItemAddWindow oItemAddWindow = null;
        private ItemList oTodoItemList = null;
        private ItemList oDoneItemList = null;
        private List<Tab> tabs = new List<Tab>();
        private List<TodoItem> oShowTodoList = null;
        private Constants.MainWindowStatu statu = Constants.MainWindowStatu.TODO;
        private Constants.MainWindowLockStatu locked = Constants.MainWindowLockStatu.DRAGABLE;
        private Syncer oSyncThread = null;
        private Setting oSetting = null;
        
        private static readonly string _regPath = @"Software\TodoPlus\";

        private int iMaxIndex = 0;
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
        
        /// <summary>
        /// 窗口加载事件，执行对应的操作：
        /// 1、窗体句柄初始化，设置标题栏状态
        /// 2、读取存储的信息
        /// 3、读取存储在注册表中的配置信息
        /// </summary>
        private void MainWindow_onLoaded(object sender, EventArgs e) {
            const int GWL_STYLE = (-16);
            const UInt64 WS_CHILD = 0x40000000;
            UInt64 iWindowStyle = GetWindowLong(hMainWindowHandle, GWL_STYLE);
            SetWindowLong(hMainWindowHandle, GWL_STYLE,
                (iWindowStyle | WS_CHILD));

            eTodoButtonStatu = Visibility.Visible;
            eDoneButtonStatu = Visibility.Collapsed;
            
            
            // todo: 封装为一个函数
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

            oSetting = new Setting();
            oSetting.ReadSettingFromRegistryTable();
            MainSetSize(this, oSetting);
            MainWindowAppearanceLoadSetting(oSetting);
            StartSyncThread();
        }
        
        /// <summary>
        /// 关闭窗口时的事件函数，保存配置到注册表
        /// </summary>
        private void MainWindow_onClosing(object sender, EventArgs e) {
            oSetting.SaveSettingToRegistryTable(this);
        }
        
        /// <summary>
        /// 关闭窗口后的事件函数：将用户数据存储到磁盘
        /// </summary>
        private void MainWindow_onClosed(object sender, EventArgs e) {

            MainWindowSaveItems();
        }

        private void MainWindowSaveItems(bool bVersionInc = true) {
            IOInterface io = new BinaryIO();
            Save save = new Save();
            save.todolist = oTodoItemList.GetItemListForSerializer();
            save.donelist = oDoneItemList.GetItemListForSerializer();
            save.tabs = tabs;
            if (bVersionInc) {
                save.version = iSaveVersion + 1;
                iSaveVersion++;
            }
            else {
                save.version = iSaveVersion;
            }

            io.SaveToFile(ref save, Constants.SAVE_FILEPATH);
        }

        public void ThreadSaveItems() {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                this.MainWindowSaveItems(false);
            }));
        }

        public void ThreadRefreshItems() {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
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

                if (statu == Constants.MainWindowStatu.TODO) {
                    oShowTodoList = oTodoItemList.GetItemList();
                }
                else {
                    oShowTodoList = oDoneItemList.GetItemList();
                }
                todoList.ItemsSource = oShowTodoList;
                todoList.Items.Refresh();
            }));
        }
        
        /// <summary>
        ///  窗口resize时的事件函数，用于设置listbox的高度
        /// </summary>
        private void MainWindow_onResize(object sender, EventArgs e) {
            this.todoList.Height = this.Height - 95;
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
        }
        
        /// <summary>
        /// 主窗口背景修改时，修改文字的颜色保证可见
        /// </summary>
        /// <param name="color">主窗口将要修改的颜色值</param>
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
            // 修改值后刷新列表
            todoList.Items.Refresh();
        }
        
        /// <summary>
        /// 待办或已办按钮被点击时切换到对应的列表
        /// 使用SwitchItemList函数来进行切换
        /// </summary>
        private void TodoButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.TODO) return;
            SwitchItemList();
        }

        private void DoneButton_onClicked(object sender, EventArgs e) {
            if (statu == Constants.MainWindowStatu.DONE) return;
            SwitchItemList();
        }

        private void LockWindowButton_onClicked(object sender, RoutedEventArgs e) => SwitchWindowLockStatus();
        
        
        /// <summary>
        /// 切换锁定状态，在点击lock按钮时处理
        /// </summary>
        private void SwitchWindowLockStatus() {
            MainWindowAppearanceLoadSetting(oSetting);
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
        /// 事项的DoneButton被点击时将其移动到对应列表
        /// </summary>
        private void ItemDoneButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oTodoItemList.DoneOrRevertItem(button.Index, ref oDoneItemList);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
            MainWindowSaveItems();
        }
        
        /// <summary>
        /// 事项的RevertButton被点击时将其移动到对应列表
        /// </summary>
        private void ItemRevertButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oDoneItemList.DoneOrRevertItem(button.Index, ref oTodoItemList);
            oShowTodoList = oDoneItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
            MainWindowSaveItems();
        }

        private void ItemDeleteButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oTodoItemList.DeleteItem(button.Index);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
            MainWindowSaveItems();
        }

        public void StickItemButton_onClicked(object sender, RoutedEventArgs e) {
            IconButton button = (IconButton)sender;
            oTodoItemList.SetItemToTop(button.Index);
            oShowTodoList = oTodoItemList.GetItemList();
            todoList.ItemsSource = oShowTodoList;
            todoList.Items.Refresh();
            MainWindowSaveItems();
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (locked == Constants.MainWindowLockStatu.LOCKED) return;
            base.DragMove();
        }
        
        /// <summary>
        /// 打开设置窗口，先设置一系列的回调函数用于配置响应
        /// </summary>
        public void OpenSettingWindow(object sender, RoutedEventArgs e) {
            if (oSettingWindow != null) {
                oSettingWindow.ShowDialog();
                return;
            }

            oSettingWindow = new SettingWindow();
            // 备份一份设置扔给设置窗口
            oSettingWindow.setting = new Setting(oSetting);
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
        
        /// <summary>
        /// 打开一个Item添加窗口
        /// </summary>
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
        
        /// <summary>
        /// 设置窗口关闭回调，将窗口设置为null，并且焦点回到当前窗口
        /// </summary>
        private void SettingWindow_OnClosed() {
            oSettingWindow = null;
            MainWindowAppearanceLoadSetting(oSetting);
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
            MainWindowSaveItems();
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
            oSetting = oNewSetting;
            MainWindowAppearanceLoadSetting(oSetting);
            if (oNewSetting.eSyncerType != Constants.SyncerType.NONE) {
                StartSyncThread();
            }
        }

        private void MainWindowRollbackSetting() {
            MainWindowAppearanceLoadSetting(oSetting);
        }

        public void TabAddEvent(Tab oNewTab) {
            tabs.Add(oNewTab);
        }
        
        
        /// <summary>
        /// 在无边框窗口下用于进行大小调整的解决方案
        /// </summary>
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
            return oSetting.CloseTips;
        }

        private void StartSyncThread() {
            if (oSyncThread == null && oSetting.eSyncerType != Constants.SyncerType.NONE) {
                oSyncThread = new Syncer();
                oSyncThread.Initial(oSetting.appid, oSetting.secretId, oSetting.secretKey, oSetting.region, oSetting.bucket);
                oSyncThread.ThreadRefreshItemsCallback += ThreadRefreshItems;
                oSyncThread.ThreadSaveItemsCallback += ThreadSaveItems;
                oSyncThread.SyncMainThread();
            }
        }

        public void SynceWindowButton_onClicked(object sender, RoutedEventArgs e) {
            MainWindowSaveItems(true);
            oSyncThread.SyncFileNow();
        }
    }

    // !! subClass Define
}