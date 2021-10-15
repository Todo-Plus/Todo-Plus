using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TodoListCSharp.controls {
    /// <summary>
    /// MenuItem 菜单项
    /// params：
    //MenuItem_OnLoadedageSource
    /// @Text： string
    /// @FontSize: int
    /// @FontColor: Color
    /// 
    /// functions：
    /// @ Click： function
    /// </summary>
    public partial class MenuItem : UserControl {
        // !! Property Functions Define
        public ImageSource IconSource {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // !! Properties Register
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource",
            typeof(ImageSource), typeof(MenuItem));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(MenuItem));
        public MenuItem() {
            InitializeComponent();
        }

        // !! delegate & event define
        public delegate void ClickEventArgs(object sender, RoutedEventArgs e);
        public event ClickEventArgs Click;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            if (Click != null) {
                Click(sender, e);
            }
        }

        public class MenuItemModel {
            public ImageSource IconSource { get; set; }
            public string Text { get; set; }
        }

        private void MenuItem_OnLoaded(object sender, RoutedEventArgs e) {
            var data = new MenuItemModel() {
                IconSource = IconSource,
                Text = Text
            };
            this.DataContext = data;
        }
    }
}
