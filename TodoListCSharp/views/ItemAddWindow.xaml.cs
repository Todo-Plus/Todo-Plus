using System;
using System.Collections.Generic;
using System.Windows;
using TodoListCSharp.core;

namespace TodoListCSharp.views {
    public partial class ItemAddWindow : Window {
        // !! delegate & event define

        public delegate void CloseCallbackFunc();

        public event CloseCallbackFunc closeCallbackFunc;

        public delegate void AddItemToListFunc(ref TodoItem item);

        public event AddItemToListFunc AddItemToList;

        public ItemAddWindow(List<Tab> tabs) {
            InitializeComponent();

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
            this.titlebar.CloseButtonClicked += CloseItemAddWindow;

            this.ComboBox.ItemsSource = tabs;
        }

        private void ItemAddWindow_onClosed(object sender, EventArgs e) {
            if (closeCallbackFunc != null) {
                closeCallbackFunc();
            }
        }

        private void DoneButton_onClick(object sender, EventArgs e) {
            string title = TitleInput.Text;
            string desc = DescInput.Text;
            TodoItem item = new TodoItem(title, desc);
            AddItemToList(ref item);
            this.Close();
        }

        private void CloseItemAddWindow(object sender, EventArgs e) {
            Close();
        }
    }
}