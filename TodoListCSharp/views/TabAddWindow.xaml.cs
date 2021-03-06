using System;
using System.Windows;
using System.Windows.Media;
using TodoListCSharp.utils;

namespace TodoListCSharp.views {
    public partial class TabAddWindow : Window {
        private Color oSelectColor = Color.FromRgb(255, 255, 255);

        public TabAddWindow() {
            InitializeComponent();

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
        }

        public delegate void ConfirmButtonCallbackFunc(string sTabTitle, Color sTabColor);

        public event ConfirmButtonCallbackFunc ConfirmButtonCallback;

        public delegate void CancelButtonCallbackFunc();

        public event CancelButtonCallbackFunc CancelButtonCallback;

        public delegate void CloseCallbackFunc();

        public CloseCallbackFunc CloseCallback;
        
        /// <summary>
        /// 如果输入的name为空，显示提示
        /// </summary>
        public void ConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            if (this.TitleTextBox.Text == String.Empty) {
                MessageBox messageBox = new MessageBox("Please input the new tab`s name.");
                Console.Beep();
                messageBox.ShowDialog();
                return;
            }

            if (ConfirmButtonCallback != null) {
                ConfirmButtonCallback(this.TitleTextBox.Text, oSelectColor);
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

        public void ColorPicker_onSelect(Color color) {
            this.oSelectColor = color;
            this.ColorShow.Fill = new SolidColorBrush(color);
            this.ColorTextBox.Text = Utils.MediaColorToHex(color);
        }

        public void TabAddWindow_onClose(object sender, EventArgs e) {
            if (CloseCallback != null) {
                CloseCallback();
            }
        }

        private void CloseButton_onClicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}