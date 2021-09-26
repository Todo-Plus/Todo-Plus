using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoListCSharp.controls {
    /// <summary>
    /// Titlebar.xaml 的交互逻辑
    /// </summary>
    public partial class Titlebar : UserControl {
        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(Titlebar));
        public Titlebar() {
            InitializeComponent();
        }

        public class TitlebarModel {
            public string Title { get; set; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            var data = new TitlebarModel() {
                Title = Title
            };
            this.DataContext = data;
        }
    }
}
