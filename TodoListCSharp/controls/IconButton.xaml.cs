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
    /// IconButton.xaml 的交互逻辑
    /// </summary>
    public partial class IconButton : UserControl {
        public IconButton() {
            InitializeComponent();
        }

        public ImageSource ImageSource {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(ImageSource), typeof(IconButton));

        private void IconButton_Loaded(object sender, RoutedEventArgs e) {
            var data = new IconButtonModel() {
                ImageSource = ImageSource
            };
            this.DataContext = data;
        }

        public delegate void ClickEventArgs(object sender, RoutedEventArgs e);
        public event ClickEventArgs Click;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            if (Click != null) {
                Click(sender, e);
            }
        }


        public class IconButtonModel {
            public ImageSource ImageSource { get; set; }
        }
    }
}
