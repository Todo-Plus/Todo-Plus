using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TodoListCSharp.core;
using TodoListCSharp.utils;
using TodoPlus.views;

namespace TodoListCSharp.views {
    /// <summary>
    /// 设置窗口交互逻辑，将三个窗口融合后需要分配不同的命名方式
    /// 主设置窗口：Main_xxx
    /// 基本设置窗口：General_xxx
    /// 外观设置窗口：Appearance_xxx
    /// 备份设置窗口：Backup_xxx
    /// </summary>
    public partial class SettingWindow : Window {
        // !! Property Define
        
        /// <summary>
        /// 一系列属性声明：
        /// 窗口： Tab添加窗口
        /// 常数： Tab的最大Index，用于进行编号，保证自增
        /// 设置： 当前设置和备份设置，备份设置用于恢复状态
        /// 列表： tabs的列表
        /// </summary>
        private TabAddWindow oGeneralTabAddWindow;

        private BackupEditWindow oBackupEditWindow;
        private int iGeneralTabLastestIndex;

        public Setting setting = null;
        private Setting oBackupSetting = null;
        public List<Tab> tabs;

        private bool bAppearanceDraging = false;
        private const string QuickName = "TodoPlusClient";
        private string systemStartPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.Startup); } }
        private string appAllPath { get { return Process.GetCurrentProcess().MainModule.FileName; } }
        private string desktopPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); } }

        // !! Events & delegate Define
        public delegate void SettingWindowClosed();
        
        // 设置窗口的回调函数
        public event SettingWindowClosed settingWindowClosed;

        // Appearance Setting Window Delegates

        public delegate void AppearanceSettingChangeCallbackFunc(Setting setting);
        
        // 外观变动时在主窗口进行显示
        public event AppearanceSettingChangeCallbackFunc AppearanceSettingChangeCallback;
        public delegate void RollbackSettingCallbackFunc();
        // 点击取消或者返回时，退回到原设置
        public event RollbackSettingCallbackFunc RollbackSettingCallback;
        
        public delegate void SettingConfirmCallbackFunc(Setting setting);
        // setting点击确认时的回调函数
        public event SettingConfirmCallbackFunc SettingConfirmCallback;

        // General Setting Window Delegates

        public delegate void TabAddCallbackFunc(Tab oNewTab);

        public event TabAddCallbackFunc TabAddCallback;

        // !! Functions Define and Implement
        public SettingWindow() {
            InitializeComponent();
            MainTitleBar.CollapseReturnButton();
        }
        
        /// <summary>
        /// load后的事件函数，处理相关状态
        /// </summary>
        private void MainSettingWindow_onLoaded(object sender, EventArgs e) {
            AppearancePercentLabel.Content = setting.Alpha;
            AppearanceTransparencySlider.Value = setting.Alpha;

            GeneralAutoStartCheckBox.IsChecked = setting.AutoRun;
            GeneralCloseMessage.IsChecked = setting.CloseTips;
            GeneralPageStackPanelInitial(tabs);
        }

        private void SettingWindow_OnClosed(object sender, EventArgs e) {
            if (settingWindowClosed != null) {
                settingWindowClosed();
            }
        }

        private void SettingWindowClose(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// General窗口打开响应事件，在点击后窗口切换到该页面
        /// </summary>
        private void GeneralItem_onClicked(object sender, RoutedEventArgs e) {
            MainPageStackPanel.Visibility = Visibility.Collapsed;
            GeneralPageStackPanel.Visibility = Visibility.Visible;
            oBackupSetting = new Setting(setting);
        }

        /// <summary>
        /// Appearance窗口打开响应事件，在点击后窗口切换到该页面
        /// </summary>
        private void AppearanceItem_onClicked(object sender, RoutedEventArgs e) {
            MainPageStackPanel.Visibility = Visibility.Collapsed;
            AppearancePageStackPanel.Visibility = Visibility.Visible;
            oBackupSetting = new Setting(setting);
        }

        /// <summary>
        /// Backup窗口打开响应事件，在点击后窗口切换到该页面
        /// </summary>
        private void BackupItem_onClicked(object sender, RoutedEventArgs e) {
            MainPageStackPanel.Visibility = Visibility.Collapsed;
            BackupPageStackPanel.Visibility = Visibility.Visible;
            oBackupSetting = new Setting(setting);
        }

        /// <summary>
        /// 从子页面返回到主页面事件,这里等同于子窗口下的Cancel按钮，如果存在状态信息需要清除
        /// </summary>
        private void ReturnButton_onClicked(object sender, RoutedEventArgs e) {
            ReturnMainSettingWindow();
        }

        /// <summary>
        /// 基本设置界面下的Add按钮被按下时的响应事件
        /// </summary>
        private void GeneralAddButton_onClicked(object sender, RoutedEventArgs e) {
            if (oGeneralTabAddWindow != null) return;
            oGeneralTabAddWindow = new TabAddWindow();
            oGeneralTabAddWindow.Owner = this;
            oGeneralTabAddWindow.ConfirmButtonCallback += GeneralAddTabWindowConfirm_onClicked;
            oGeneralTabAddWindow.CloseCallback += GeneralAddTabWindowConfirm_onClose;
            oGeneralTabAddWindow.ShowDialog();
        }
        
        /// <summary>
        /// 基础设置确认按钮点击响应事件，添加tab
        /// </summary>
        private void GeneralAddTabWindowConfirm_onClicked(string sTabTitle, Color sTabColor) {
            GeneralWrapPanel.Children.Add(
                Generator.TabViewGridGenerate(sTabTitle, sTabColor));
            if (TabAddCallback != null) {
                Tab oNewTab = new Tab(iGeneralTabLastestIndex, sTabTitle, sTabColor);
                TabAddCallback(oNewTab);
            }
        }

        private void GeneralAddTabWindowConfirm_onClose() {
            this.Activate();
        }

        /// <summary>
        /// General页面下的初始化函数
        /// </summary>
        /// <param name="tabs">General下的tab列表，用于进行初始化</param>
        public void GeneralPageStackPanelInitial(List<Tab> tabs) {
            int length = tabs.Count;
            for (int i = 0; i < length; i++) {
                this.GeneralWrapPanel.Children.Add(
                    Generator.TabViewGridGenerate(tabs[i].Name, tabs[i].GetColor()));
            }

            iGeneralTabLastestIndex = length > 0 ? tabs[length - 1].Id + 1 : 0;
        }
        
        /// <summary>
        /// 在外观的透明度被修改时，显示到lab，并且主窗口进行对应的显示
        /// </summary>
        /// <param name="value">透明度</param>
        private void AppearanceSetShowPercentValue(int value) {
            AppearancePercentLabel.Content = value;
            setting.Alpha = value;
            AppearanceSettingChange(setting);
        }
        
        /// <summary>
        /// slider的相关事件
        /// </summary>
        private void AppearanceSlider_onDragStart(object sender, DragStartedEventArgs e) {
            bAppearanceDraging = true;
        }

        private void AppearanceSlider_onDragEnd(object sender, DragCompletedEventArgs e) {
            AppearanceSetShowPercentValue((int)((Slider)sender).Value);
            bAppearanceDraging = false;
        }

        private void AppearanceSlider_onValueChange(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (bAppearanceDraging) {
                AppearanceSetShowPercentValue((int)e.NewValue);
            }
        }

        private void AppearanceColorPicker_onColorChange(Color color) {
            setting.BackgroundColor = color;
            AppearanceSettingChange(setting);
        }

        // !! delegate forward
        /// <summary>
        /// 一系列的委托转发，用于处理窗口和主窗口之间传递的事件
        /// </summary>
        /// <param name="setting"></param>
        private void AppearanceSettingChange(Setting setting) {
            if (AppearanceSettingChangeCallback != null) {
                AppearanceSettingChangeCallback(setting);
            }
        }

        private void CloseButton_onClicked(object sender, RoutedEventArgs e) {
            if (RollbackSettingCallback != null) {
                RollbackSettingCallback();
                setting = oBackupSetting;
                ReturnMainSettingWindow();
            }
            this.Close();
        }

        private void SettingCancelButton_onClicked(object sender, RoutedEventArgs e) {
            if (RollbackSettingCallback != null) {
                RollbackSettingCallback();
                setting = oBackupSetting;
                ReturnMainSettingWindow();
            }
        }

        private void SettingConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            if (SettingConfirmCallback != null) {
                SettingConfirmCallback(setting);
                oBackupSetting = setting;
                ReturnMainSettingWindow();
            }
        }
        
        /// <summary>
        /// 点击返回按钮时，切换到设置窗口
        /// todo：添加动画
        /// </summary>
        private void ReturnMainSettingWindow() {
            GeneralPageStackPanel.Visibility = Visibility.Collapsed;
            AppearancePageStackPanel.Visibility = Visibility.Collapsed;
            BackupPageStackPanel.Visibility = Visibility.Collapsed;

            MainPageStackPanel.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// 自动启动开关被点击时，进行对应的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralAutoRunCheckbox_onChecked(object sender, RoutedEventArgs e) {
            setting.AutoRun = true;

            List<string> shortcutPaths = Utils.GetQuickFromFolder(systemStartPath, appAllPath);
            if (shortcutPaths.Count >= 2) {
                for (int i = 1; i < shortcutPaths.Count; i++) {
                    Utils.DeleteFile(shortcutPaths[i]);
                }
            }
            else if (shortcutPaths.Count < 1)//不存在则创建快捷方式
            {
                Utils.CreateShortcut(systemStartPath, QuickName, appAllPath, "Todo+");
            }
            if (SettingConfirmCallback != null) {
                SettingConfirmCallback(setting);
            }
        }
        
        private void GeneralAutoRunCheckbox_onUnChecked(object sender, RoutedEventArgs e) {
            setting.AutoRun = false;

            List<string> shortcutPaths = Utils.GetQuickFromFolder(systemStartPath, appAllPath);

            if (shortcutPaths.Count > 0) {
                for (int i = 0; i < shortcutPaths.Count; i++) {
                    Utils.DeleteFile(shortcutPaths[i]);
                }
            }

            if (SettingConfirmCallback != null) {
                SettingConfirmCallback(setting);
            }
        }
        
        /// <summary>
        /// 切换提示状态：Exit时是否显示提示
        /// </summary>
        private void GeneralTipsMessageCheckbox_onChecked(object sender, RoutedEventArgs e) {
            setting.CloseTips = true;
            if (SettingConfirmCallback != null) {
                SettingConfirmCallback(setting);
            }
        }

        private void GeneralTipsMessageCheckbox_onUnChecked(object sender, RoutedEventArgs e) {
            setting.CloseTips = false;
            if (SettingConfirmCallback != null) {
                SettingConfirmCallback(setting);
            }
        }
        
        /// <summary>
        /// 设置备份响应事件
        /// </summary>
        private void TencentRadioButton_onClicked(object sender, RoutedEventArgs e) {
            if (oBackupEditWindow != null) return;
            oBackupEditWindow = new BackupEditWindow(setting);
            oBackupEditWindow.BackupEditCloseCallback += BackupEditWindow_onClosed;
            oBackupEditWindow.ConfirmButtonClickCallback += BackupSettingWindow_Confirm;
            oBackupEditWindow.Owner = this;
            oBackupEditWindow.ShowDialog();
        }

        private void BackupEditWindow_onClosed() {
            oBackupEditWindow = null;
        }

        private void BackupSettingWindow_Confirm(string appid, string sid, string skey, string region, string bucket) {
            setting.eSyncerType = Constants.SyncerType.TENCENTCOS;
            setting.appid = appid;
            setting.secretId = sid;
            setting.secretKey = skey;
            setting.region = region;
            setting.bucket = bucket;
            SettingConfirmCallback?.Invoke(setting);
        }
    }
}