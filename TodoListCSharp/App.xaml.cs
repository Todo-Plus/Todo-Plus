using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using TodoListCSharp.core;
using Application = System.Windows.Application;
using MessageBox = TodoListCSharp.views.MessageBox;

namespace TodoListCSharp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private NotifyIcon oTrayIcon = new NotifyIcon();
        private Setting Settig;

        public App() {
            oTrayIcon.Icon = new System.Drawing.Icon("./resources/todo.ico");
            oTrayIcon.Visible = true;
            oTrayIcon.Text = "Todo+";
            oTrayIcon.DoubleClick += NotifyIcon_onDoubleClick;

            oTrayIcon.ContextMenuStrip = new ContextMenuStrip();
            oTrayIcon.ContextMenuStrip.Items.Add("Setting", null, this.MenuItem_OpenSetting);
            oTrayIcon.ContextMenuStrip.Items.Add("Exit", null, this.MenuItem_ExitApplication);

        }

        public void MenuItem_OpenSetting(object sender, EventArgs e) {
            TodoListCSharp.MainWindow oMainWindow = (TodoListCSharp.MainWindow)Application.Current.MainWindow;
            oMainWindow.OpenSettingWindow(sender, null);
        }

        public void MenuItem_ExitApplication(object sender, EventArgs e) {
            TodoListCSharp.MainWindow oMainWindow = (TodoListCSharp.MainWindow)Application.Current.MainWindow;
            if (oMainWindow.ShowTipMessage()) {
                MessageBox messageBox = new MessageBox("You sure you want to exit the program.");
                messageBox.ConfirmButtonCallback += this.Shutdown;
                messageBox.ShowDialog();
            }
            else {
                Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            oTrayIcon.Visible = false;
        }

        private void NotifyIcon_onDoubleClick(object sender, EventArgs e) {
            TodoListCSharp.MainWindow oMainWindow = (TodoListCSharp.MainWindow)Application.Current.MainWindow;
            oMainWindow.Activate();
        }

        protected override void OnStartup(StartupEventArgs e) {
            Process process = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(process.ProcessName)) {
                if (p.Id != process.Id) {
                    Application.Current.Shutdown();
                    return;
                }
            }
            base.OnStartup(e);
        }
    }
}
