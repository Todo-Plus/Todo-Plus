using System;
using System.Collections.Generic;
using System.Windows;
using TodoListCSharp.core;

namespace TodoListCSharp.views {
    public partial class ItemAddWindow : Window {
        // !! delegate & event define
        
        /// <summary>
        /// 关闭添加Item的窗口的回调函数
        /// </summary>
        public delegate void CloseCallbackFunc();

        public event CloseCallbackFunc closeCallbackFunc;
        /// <summary>
        /// 添加项目到ItemList的回调函数
        /// </summary>
        /// <param name="item">新的Item</param>
        public delegate void AddItemToListFunc(ref TodoItem item);

        public event AddItemToListFunc AddItemToList;
        
        /// <summary>
        /// 构造函数，需要传入tab用于进行选择
        /// </summary>
        /// <param name="tabs">传入用于选择tab的列表</param>
        public ItemAddWindow(List<Tab> tabs) {
            InitializeComponent();

            this.titlebar.ReturnButton.Visibility = Visibility.Collapsed;
            this.titlebar.CloseButtonClicked += CloseItemAddWindow;

            this.ComboBox.ItemsSource = tabs;
        }
        
        /// <summary>
        /// 关闭时调用回调函数
        /// </summary>
        private void ItemAddWindow_onClosed(object sender, EventArgs e) {
            if (closeCallbackFunc != null) {
                closeCallbackFunc();
            }
        }
        
        /// <summary>
        /// 确认按钮点击响应函数，如果标题为空则弹出提示框
        /// </summary>
        private void ConfirmButton_onClicked(object sender, EventArgs e) {
            string title = TitleInput.Text;
            string desc = DescInput.Text;

            if (title == String.Empty) {
                MessageBox messageBox = new MessageBox("Please input title for item.");
                messageBox.ShowDialog();
                return;
            }

            TodoItem item = new TodoItem(title, desc);
            AddItemToList(ref item);
            this.Close();
        }

        private void CancelButton_onClicked(object sender, EventArgs e) {
            this.Close();
        }

        private void CloseItemAddWindow(object sender, EventArgs e) {
            Close();
        }
    }
}