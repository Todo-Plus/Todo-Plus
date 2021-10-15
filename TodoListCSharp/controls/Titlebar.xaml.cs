using System.Windows;
using System.Windows.Controls;

namespace TodoListCSharp.controls {
    /// <summary>
    /// Titlebar.xaml 的交互逻辑
    /// </summary>
    public partial class Titlebar : UserControl {

        // events & delegate define
        public delegate void ReturnButtonClickedEventArgs(object sender, RoutedEventArgs e);
        public event ReturnButtonClickedEventArgs ReturnButtonClicked;
        public delegate void CloseButtonClickedEventArgs(object sender, RoutedEventArgs e);
        public event CloseButtonClickedEventArgs CloseButtonClicked;
        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(Titlebar));
        public Titlebar() {
            InitializeComponent();
        }

        public void CollapseReturnButton() {
            this.ReturnButton.Visibility = Visibility.Collapsed;
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

        private void TitleBar_onReturnButtonClicked(object sender, RoutedEventArgs e) {
            if (ReturnButtonClicked != null) {
                ReturnButtonClicked(sender, e);
            }
        }

        private void TitleBar_onCloseButtonClicked(object sender, RoutedEventArgs e) {
            if (CloseButtonClicked != null) {
                CloseButtonClicked(sender, e);
            }
        }
    }
}
