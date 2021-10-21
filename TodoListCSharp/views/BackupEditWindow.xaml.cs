using System;
using System.Windows;
using TodoListCSharp.core;
using MessageBox = TodoListCSharp.views.MessageBox;

namespace TodoPlus.views {
    public partial class BackupEditWindow : Window {
        public delegate void BackupEditCloseCallbackFunc();

        public delegate void ConfirmButtonClickCallbackFunc(string appid, string sid, string skey, string region, string bucket);
        
        public event BackupEditCloseCallbackFunc BackupEditCloseCallback;
        public event ConfirmButtonClickCallbackFunc ConfirmButtonClickCallback;
        public BackupEditWindow(Setting setting) {
            InitializeComponent();

            this.AppIDInput.Text = setting.appid;
            this.SecretIDInput.Text = setting.secretId;
            this.SecretKeyInput.Text = setting.secretKey;
            this.RegionInput.Text = setting.region;
            this.BucketInput.Text = setting.bucket;
        }

        public void ConnectButton_onClicked(object sender, RoutedEventArgs e) {
            
        }
        public void ConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            string appid = this.AppIDInput.Text;
            string sid = this.SecretIDInput.Text;
            string skey = this.SecretKeyInput.Text;
            string region = this.RegionInput.Text;
            string bucket = this.BucketInput.Text;
            if (appid == string.Empty ||
                sid == string.Empty ||
                skey == string.Empty ||
                region == string.Empty ||
                bucket == string.Empty) {
                MessageBox messageBox = new MessageBox("Please fill in the information completely.");
                messageBox.ShowDialog();
                return;
            }
            ConfirmButtonClickCallback?.Invoke(appid, sid, skey, region, bucket);
            this.Close();
        }        
        public void CancelButton_onClicked(object sender, RoutedEventArgs e) {
            this.Close();
        }

        public void BackupEditWindow_onClosed(object sender, EventArgs e) => BackupEditCloseCallback?.Invoke();
    }
}