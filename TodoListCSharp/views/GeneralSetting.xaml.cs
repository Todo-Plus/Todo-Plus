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
    /// GeneralSetting.xaml 的交互逻辑
    /// </summary>
    public partial class GeneralSetting : Window {
        // !! Events & delegate Define
        public delegate void ClosedCallbackFunc();
        public event ClosedCallbackFunc closedCallbackFunc;
        
        public GeneralSetting() {
            InitializeComponent();
        }

        private void GeneralSetting_onClosed(object sender, EventArgs e) {
            if (closedCallbackFunc != null) {
                closedCallbackFunc();
            }
        }
    }
}
