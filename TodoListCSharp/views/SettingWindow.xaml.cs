using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoListCSharp.core;
using TodoListCSharp.utils;
using TodoListCSharp.views;

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

        private GeneralSetting oGeneralSettingWindow = null;
        private AppearanceSetting oAppearanceSettingWindow = null;
        private BackupSetting oBackupSettingWindow = null;

        private TabAddWindow oGeneralTabAddWindow;
        private int iGeneralTabLastestIndex;

        public Setting setting = null;
        public List<Tab> tabs;

        private bool bAppearanceDraging = false;

        // !! Events & delegate Define
        public delegate void SettingWindowClosed();

        public event SettingWindowClosed settingWindowClosed;

        // Appearance Setting Window Delegates

        public delegate void AppearanceSettingChangeCallbackFunc(Setting setting);

        public event AppearanceSettingChangeCallbackFunc AppearanceSettingChangeCallback;

        // General Setting Window Delegates

        public delegate void TabAddCallbackFunc(Tab oNewTab);

        public event TabAddCallbackFunc TabAddCallback;

        // !! Functions Define and Implement
        public SettingWindow() {
            InitializeComponent();
            MainTitleBar.CollapseReturnButton();

            // Set Close Button Callback Function
            // this.InfoMenuItem.Click += OpenGeneralSettingWindow;
            // this.AppearanceMenuItem.Click += OpenAppearanceSettingWindow;
            // this.BackupMenuItem.Click += openBackupSettingWindow;
        }

        private void MainSettingWindow_onLoaded(object sender, EventArgs e) {
            AppearancePercentLabel.Content = setting.Alpha;
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
        }
        
        /// <summary>
        /// Appearance窗口打开响应事件，在点击后窗口切换到该页面
        /// </summary>
        private void AppearanceItem_onClicked(object sender, RoutedEventArgs e) {
            MainPageStackPanel.Visibility = Visibility.Collapsed;
            AppearancePageStackPanel.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Backup窗口打开响应事件，在点击后窗口切换到该页面
        /// </summary>
        private void BackupItem_onClicked(object sender, RoutedEventArgs e) {
            MainPageStackPanel.Visibility = Visibility.Collapsed;
            BackupPageStackPanel.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// 从子页面返回到主页面事件,这里等同于子窗口下的Cancel按钮，如果存在状态信息需要清除
        /// </summary>
        private void ReturnButton_onClicked(object sender, RoutedEventArgs e) {
            GeneralPageStackPanel.Visibility = Visibility.Collapsed;
            AppearancePageStackPanel.Visibility = Visibility.Collapsed;
            BackupPageStackPanel.Visibility = Visibility.Collapsed;

            MainPageStackPanel.Visibility = Visibility.Visible;
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

        private void GeneralAddTabWindowConfirm_onClicked(string sTabTitle, Color sTabColor) {
            GeneralWrapPanel.Children.Add(
                Generator.TabViewGridGenerate(sTabTitle, sTabColor));
            if (TabAddCallback != null) {
                Tab oNewTab = new Tab(iGeneralTabLastestIndex, sTabTitle, sTabColor);
                TabAddCallback(oNewTab);
            }
        }

        private void GeneralAddTabWindowConfirm_onClose() {
            oGeneralSettingWindow = null;
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

        private void AppearanceSetShowPercentValue(int value) {
            AppearancePercentLabel.Content = value;
            setting.Alpha = value;
            AppearanceSettingChange(setting);
        }

        private void AppearanceSlider_onDragStart(object sender, DragStartedEventArgs e) {
            bAppearanceDraging = true;
        }

        private void AppearanceSlider_onDragEnd(object sender, DragCompletedEventArgs e) {
            AppearanceSetShowPercentValue((int) ((Slider) sender).Value);
            bAppearanceDraging = false;
        }

        private void AppearanceSlider_onValueChange(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (bAppearanceDraging) {
                AppearanceSetShowPercentValue((int) e.NewValue);
            }
        }

        private void AppearanceConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            
        }

        private void AppearanceCancelButton_onClicked(object sender, RoutedEventArgs e) {
            // todo: 清空页面状态
        }
        private void OpenGeneralSettingWindow(object sender, EventArgs e) {
            if (oGeneralSettingWindow != null) {
                return;
            }

            oGeneralSettingWindow = new GeneralSetting(ref tabs);
            oGeneralSettingWindow.closedCallbackFunc += CloseGeneralSettingWindow;
            oGeneralSettingWindow.TabAddCallback += GeneralSetting_AddTab;
            oGeneralSettingWindow.Owner = this;
            oGeneralSettingWindow.ShowDialog();
        }

        private void CloseGeneralSettingWindow() {
            oGeneralSettingWindow = null;
            this.Activate();
        }

        private void OpenAppearanceSettingWindow(object sender, EventArgs e) {
            if (oAppearanceSettingWindow != null) {
                return;
            }

            oAppearanceSettingWindow = new AppearanceSetting();
            oAppearanceSettingWindow.Show();
            oAppearanceSettingWindow.setting = setting;
            oAppearanceSettingWindow.closedCallbackFunc += CloseAppearanceSettingWindow;
            oAppearanceSettingWindow.AppearanceSettingChange += AppearanceSettingChange;
            oAppearanceSettingWindow.Owner = this;
        }

        private void CloseAppearanceSettingWindow() {
            oGeneralSettingWindow = null;
            this.Activate();
        }

        private void openBackupSettingWindow(object sender, EventArgs e) {
            if (oBackupSettingWindow != null) {
                return;
            }

            oBackupSettingWindow = new BackupSetting();
            oBackupSettingWindow.Show();
            oBackupSettingWindow.closedCallbackFunc += CloseBackupSettingWindow;
            oBackupSettingWindow.Owner = this;
        }

        private void CloseBackupSettingWindow() {
            oBackupSettingWindow = null;
            this.Activate();
        }

        // !! delegate forward
        private void AppearanceSettingChange(Setting setting) {
            if (AppearanceSettingChangeCallback != null) {
                AppearanceSettingChangeCallback(setting);
            }
        }

        private void GeneralSetting_AddTab(Tab oNewTab) {
            if (TabAddCallback != null) {
                TabAddCallback(oNewTab);
            }
        }

        private void CloseButton_onClicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}