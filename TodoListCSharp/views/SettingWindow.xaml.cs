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
        
        // !! Events & delegate Define
        public delegate void SettingWindowClosed();
        public event SettingWindowClosed settingWindowClosed;
        
        // !! Functions Define and Implement
        public SettingWindow() {
            InitializeComponent();
            titlebar.CollapseReturnButton();
            
            // Set Close Button Callback Function
            this.titlebar.CloseButtonClicked += SettingWindowClose;
            this.InfoMenuItem.Click += OpenGeneralSettingWindow;
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
            oGeneralSettingWindow = new GeneralSetting();
            oGeneralSettingWindow.Show();
            oGeneralSettingWindow.closedCallbackFunc += CloseGeneralSettingWindow;
            oGeneralSettingWindow.Owner = this;
        }

        private void CloseGeneralSettingWindow() {
            oGeneralSettingWindow = null;
        }
    }
}
