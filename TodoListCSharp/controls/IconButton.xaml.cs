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
        /// <summary>
        ///  自定义相关属性函数声明
        /// </summary>
        public ImageSource ImageSource {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public ImageSource HoverIcon {
            get { return (ImageSource)GetValue(HoverIconProperty); }
            set { SetValue(HoverIconProperty, value); }
        }
        public int IconDiameter {
            get { return (int)GetValue(IconDiameterProperty); }
            set { SetValue(IconDiameterProperty, value); }
        }
        public int CircleDiameter {
            get { return (int)GetValue(CircleDiameterProperty); }
            set { SetValue(CircleDiameterProperty, value); }
        }
        public int ItemIndex {
            get { return (int)GetValue(ItemIndexProperty); }
            set { SetValue(ItemIndexProperty, value); }
        }

        /// <summary>
        /// 自定义属性注册
        /// @ImageSource： 图像资源
        /// @IconDiameter： 图标直径
        /// @CircleDiameter： 按钮的直径
        /// @HoverIcon：鼠标悬浮时显示的按钮图像
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(ImageSource), typeof(IconButton));
        public static readonly DependencyProperty HoverIconProperty = DependencyProperty.Register("HoverIcon",
            typeof(ImageSource), typeof(IconButton));
        public static readonly DependencyProperty IconDiameterProperty = DependencyProperty.Register("IconDiameter",
            typeof(int), typeof(IconButton));
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.Register("CircleDiameter",
            typeof(int), typeof(IconButton));
        public static readonly DependencyProperty ItemIndexProperty = DependencyProperty.Register("ItemIndex", typeof(int),
            typeof(IconButton));

        private void IconButton_Loaded(object sender, RoutedEventArgs e) {
            var data = new IconButtonModel() {
                ImageSource = ImageSource,
                IconDiameter = IconDiameter,
                CircleDiameter = CircleDiameter,
                HoverIcon = HoverIcon,
                ItemIndex = ItemIndex
            };
            this.DataContext = data;
        }

        public delegate void ClickEventArgs(object sender, RoutedEventArgs e);
        public event ClickEventArgs Click;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            if (Click != null) {
                Click(this, e);
            }
        }
        
        public class IconButtonModel {
            public ImageSource ImageSource { get; set; }
            public ImageSource HoverIcon { get; set; }
            public int IconDiameter { get; set; }
            public int CircleDiameter { get; set; }
            public int ItemIndex { get; set; }
        }
    }
}
