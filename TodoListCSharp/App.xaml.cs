using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

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
        }
    }
}
