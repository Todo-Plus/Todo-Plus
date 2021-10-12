using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TodoListCSharp.views;
using Application = System.Windows.Application;
using MessageBox = TodoListCSharp.views.MessageBox;

namespace TodoListCSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private NotifyIcon oTrayIcon = new NotifyIcon();

        public App() {
            oTrayIcon.Icon = new System.Drawing.Icon("./resources/todo.ico");
            oTrayIcon.Visible = true;
            oTrayIcon.Text = "Todo+";

            oTrayIcon.ContextMenuStrip = new ContextMenuStrip();
            oTrayIcon.ContextMenuStrip.Items.Add("Setting", null, this.MenuItem_OpenSetting);
            oTrayIcon.ContextMenuStrip.Items.Add("Exit", null, this.MenuItem_ExitApplication);

        }

        public void MenuItem_OpenSetting(object sender, EventArgs e) {
            TodoListCSharp.MainWindow oMainWindow = (TodoListCSharp.MainWindow)Application.Current.MainWindow;
            oMainWindow.OpenSettingWindow(sender, null);
        }

        public void MenuItem_ExitApplication(object sender, EventArgs e) {
            MessageBox messageBox = new MessageBox("You sure you want to exit the program.");
            messageBox.ConfirmButtonCallback += this.Shutdown;
            messageBox.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            oTrayIcon.Visible = false;
        }
    }
}
