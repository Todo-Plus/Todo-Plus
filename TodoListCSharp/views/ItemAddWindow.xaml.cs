using System;
using System.Windows;

namespace TodoListCSharp.views {
    public partial class ItemAddWindow : Window {
        // !! delegate & event define

        public delegate void CloseCallbackFunc();

        public event CloseCallbackFunc closeCallbackFunc;
        
        public ItemAddWindow() {
            InitializeComponent();

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
        }

        private void ItemAddWindow_onClosed(object sender, EventArgs e) {
            if (closeCallbackFunc != null) {
                closeCallbackFunc();
            }
        }
    }
}