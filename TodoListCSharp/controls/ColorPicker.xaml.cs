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
    /// ColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPicker : UserControl {
        private const int Width = 256;
        private const int Height = 144;
        
        public Color DefaultColor = Color.FromRgb(255, 0, 255);
        public ColorPicker() {
            InitializeComponent();
            this.CoreColor.Color = DefaultColor;
        }

        private void Canvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Point postion = e.GetPosition((IInputElement) sender);
            double left = 2 * postion.X - Width;
            double top = 2 * postion.Y - Height;
            this.ClickedPos.Margin = new Thickness(left, top, 0, 0);
        }
    }
}
