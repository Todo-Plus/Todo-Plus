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

namespace TodoListCSharp.views {
    /// <summary>
    /// BackupSetting.xaml 的交互逻辑
    /// </summary>
    public partial class BackupSetting : Window {
        // delegate & events
        public delegate void ClosedCallbackFunc();
        public event ClosedCallbackFunc closedCallbackFunc;
        public BackupSetting() {
            InitializeComponent();
        }

        private void BackupSetting_OnClosed(object? sender, EventArgs e) {
            if (closedCallbackFunc != null) {
                closedCallbackFunc();
            }
        }
    }
}
