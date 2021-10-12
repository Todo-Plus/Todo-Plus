using System.Windows;

namespace TodoListCSharp.views {
    public partial class TabAddWindow : Window {
        public TabAddWindow() {
            InitializeComponent();

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
        }
        
        public delegate void ConfirmButtonCallbackFunc();

        public event ConfirmButtonCallbackFunc ConfirmButtonCallback;
        public delegate void CancelButtonCallbackFunc();

        public event CancelButtonCallbackFunc CancelButtonCallback;

        public void ConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            if (ConfirmButtonCallback != null) {
                ConfirmButtonCallback();
            }
            this.CloseMessageWindow(sender, e);
        }

        public void CancelButton_onClicked(object sender, RoutedEventArgs e) {
            if (CancelButtonCallback != null) {
                CancelButtonCallback();
            }
            this.CloseMessageWindow(sender, e);
        }

        public void CloseMessageWindow(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}