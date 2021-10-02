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

namespace TodoListCSharp.views {
    /// <summary>
    /// AppearanceSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AppearanceSetting : Window {
        
        // delegate & events
        public delegate void ClosedCallbackFunc();
        public event ClosedCallbackFunc closedCallbackFunc;
        public AppearanceSetting() {
            InitializeComponent();
        }

        private void AppearanceSetting_OnClosed(object? sender, EventArgs e) {
            if (closedCallbackFunc != null) {
                closedCallbackFunc();
            }
        }

        private void AppearanceSetting_onDragOver(object sender, DragCompletedEventArgs e) {
            System.Console.WriteLine(1);
        }
    }
}
