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
using TodoListCSharp.utils;

namespace TodoListCSharp.views {
    /// <summary>
    /// GeneralSetting.xaml 的交互逻辑
    /// </summary>
    public partial class GeneralSetting : Window {
        // !! Events & delegate Define
        public delegate void ClosedCallbackFunc();
        public event ClosedCallbackFunc closedCallbackFunc;

        public delegate void TabAddCallbackFunc(Tab oNewTab);

        public event TabAddCallbackFunc TabAddCallback;

        private TabAddWindow oTabAddWindow = null;
        private int iTabLastestIndex;

        public GeneralSetting(ref List<Tab> tabs) {
            InitializeComponent();

            int length = tabs.Count;
            for (int i = 0; i < length; i++) {
                this.WrapPanel.Children.Add(
                    Generator.TabViewGridGenerate(tabs[i].Name, tabs[i].Color));
            }

            iTabLastestIndex = length > 0 ? tabs[length - 1].Id  + 1 : 0;
        }

        private void GeneralSetting_onClosed(object sender, EventArgs e) {
            if (closedCallbackFunc != null) {
                closedCallbackFunc();
            }
        }

        private void AddButton_onClicked(object sender, EventArgs e) {
            if (oTabAddWindow != null) return;
            oTabAddWindow = new TabAddWindow();
            oTabAddWindow.Owner = this;
            oTabAddWindow.ConfirmButtonCallback += AddTabWindow_onConfirm;
            oTabAddWindow.CloseCallback += AddTabWindow_onClose;
            oTabAddWindow.Show();
        }

        private void AddTabWindow_onConfirm(string sTabTitle, Color sTabColor) {
            this.WrapPanel.Children.Add(
                Generator.TabViewGridGenerate(sTabTitle, sTabColor));
            if (TabAddCallback != null) {
                Tab oNewTab = new Tab(iTabLastestIndex, sTabTitle, sTabColor);
                TabAddCallback(oNewTab);
            }
        }
        
        private void AddTabWindow_onClose() {
            oTabAddWindow = null;
            this.Activate();
        }
    }
}
