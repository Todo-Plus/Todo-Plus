using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoListCSharp.core;
using TodoListCSharp.views;

namespace TodoListCSharp.views {
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>

    public partial class SettingWindow : Window {
        // !! Property Define

        private GeneralSetting oGeneralSettingWindow = null;
        private AppearanceSetting oAppearanceSettingWindow = null;
        private BackupSetting oBackupSettingWindow = null;
        
        public Setting setting = null;
        public List<Tab> tabs;
        
        // !! Events & delegate Define
        public delegate void SettingWindowClosed();
        public event SettingWindowClosed settingWindowClosed;
        
        // Appearance Setting Window Delegates

        public delegate void TransparencyChangeCallbackFunc(Setting setting);

        public event TransparencyChangeCallbackFunc TransparencyChangeCallback;
        
        // General Setting Window Delegates

        public delegate void TabAddCallbackFunc(Tab oNewTab);

        public event TabAddCallbackFunc TabAddCallback;
        
        // !! Functions Define and Implement
        public SettingWindow() {
            InitializeComponent();
            titlebar.CollapseReturnButton();
            
            // Set Close Button Callback Function
            this.titlebar.CloseButtonClicked += SettingWindowClose;
            this.InfoMenuItem.Click += OpenGeneralSettingWindow;
            this.AppearanceMenuItem.Click += OpenAppearanceSettingWindow;
            this.BackupMenuItem.Click += openBackupSettingWindow;
        }
        private void SettingWindow_OnClosed(object sender, EventArgs e)
        {
            if (settingWindowClosed != null)  {
                settingWindowClosed();
            }
        }
        private void SettingWindowClose(object sender, EventArgs e) {
            this.Close();
        }

        private void OpenGeneralSettingWindow(object sender, EventArgs e) {
            if (oGeneralSettingWindow != null) {
                return;
            }
            oGeneralSettingWindow = new GeneralSetting(ref tabs);
            oGeneralSettingWindow.ShowDialog();
            oGeneralSettingWindow.closedCallbackFunc += CloseGeneralSettingWindow;
            oGeneralSettingWindow.TabAddCallback += GeneralSetting_AddTab;
            oGeneralSettingWindow.Owner = this;
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
            oAppearanceSettingWindow.AppearanceSettingChange += AppearanceSetting_ChangeSetting;
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
        private void AppearanceSetting_ChangeSetting(Setting setting) {
            if (TransparencyChangeCallback != null) {
                TransparencyChangeCallback(setting);
            }
        }

        private void GeneralSetting_AddTab(Tab oNewTab) {
            if (TabAddCallback != null) {
                TabAddCallback(oNewTab);
            }
        }
    }
}
