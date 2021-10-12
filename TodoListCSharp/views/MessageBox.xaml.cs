using System.Windows;

namespace TodoListCSharp.views {
    public partial class MessageBox : Window {
        public delegate void ConfirmButtonCallbackFunc();

        public event ConfirmButtonCallbackFunc ConfirmButtonCallback;

        public delegate void CancelButtonCallbackFunc();

        public event CancelButtonCallbackFunc CancelButtonCallback;

        public MessageBox(string context) {
            InitializeComponent();
            this.context.Text = context;

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
        }

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