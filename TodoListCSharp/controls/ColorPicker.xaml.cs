using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TodoListCSharp.utils;

namespace TodoListCSharp.controls {
    /// <summary>
    /// ColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPicker : UserControl {
        private const int Width = 256;
        private const int Height = 144;
        private const int ColorSliderWidth = 230;

        private Color oRightTopColor;
        private Color oSelectColor;
        
        public Color DefaultColor = Color.FromRgb(255, 0, 255);
        public ColorPicker() {
            InitializeComponent();
            this.CoreColor.Color = DefaultColor;
        }

        private void Canvas_OnMouseMove(object sender, MouseEventArgs e) {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;
            Point postion = e.GetPosition((IInputElement) sender);
            double left = 2 * postion.X - Width;
            double top = 2 * postion.Y - Height;
            this.ClickedPos.Margin = new Thickness(left, top, 0, 0);

            double fWidthPercent = postion.X / Width;
            Debug.WriteLine(postion);
            double fHeightPercent = postion.Y / Height;

            int iStartRed = (int) (oRightTopColor.R * (1.0 - fHeightPercent));
            int iStartGreen = (int) (oRightTopColor.G * (1.0 - fHeightPercent));
            int iStartBlue = (int) (oRightTopColor.B * (1.0 - fHeightPercent));
            int iEndRed = (int) (255 * (1.0 - fHeightPercent));
            int iEndGreen = (int) (255 * (1.0 - fHeightPercent));
            int iEndBlue = (int) (255 * (1.0 - fHeightPercent));

            int iSelectRed = (int) (iEndRed - fWidthPercent * (iEndRed - iStartRed));
            int iSelectGreen = (int) (iEndGreen - fWidthPercent * (iEndGreen - iStartGreen));
            int iSelectBlue = (int) (iEndBlue - fWidthPercent * (iEndBlue - iStartBlue));

            oSelectColor = Color.FromRgb((byte) iSelectRed, (byte) iSelectGreen, (byte) iSelectBlue);
            // oSelectColor = Color.FromRgb((byte) iEndRed, (byte) iEndGreen, (byte) iEndBlue);
            // oSelectColor = Color.FromRgb((byte) iStartRed, (byte) iStartGreen, (byte) iStartBlue);
            this.ShowSelectColor.Fill = new SolidColorBrush(oSelectColor);
        }

        private void ColorSlider_onMouseMove(object sender, MouseEventArgs e) {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;
            Point position = e.GetPosition((IInputElement) sender);
            double percent = position.X / ColorSliderWidth;
            Color color = SliderPercentToColor(percent);
            this.CoreColor.Color = color;
            oRightTopColor = color;
        }
        
        private static Color SliderPercentToColor(double percent) {
            Color oRetColor;
            double fRealPercent;
            const double total = 0.167;
            if (percent < 0.167) {
                fRealPercent = percent / total;
                int Green = (int)(255 * fRealPercent);
                oRetColor = Color.FromRgb(0xFF, (byte)Green, 0x00);
            }
            else if (percent < 0.333) {
                fRealPercent = (percent - 0.167) / total;
                int Red = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb((byte)Red, 0xFF, 0x00);
            }
            else if (percent < 0.5) {
                fRealPercent = (percent - 0.333) / total;
                int Blue = (int) (fRealPercent * 255);
                oRetColor = Color.FromRgb(0x00, 0xFF, (byte)Blue);
            }
            else if (percent < 0.667) {
                fRealPercent = (percent - 0.5) / total;
                int Green = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb(0x00, (byte)Green, 0xFF);
            }
            else if (percent < 0.833) {
                fRealPercent = (percent - 0.667) / total;
                int Red = (int) (fRealPercent * 255);
                oRetColor = Color.FromRgb((byte)Red, 0x00, 0xFF);
            }
            else {
                fRealPercent = (percent - 0.833) / total;
                int Blue = (int) ((1.0 - fRealPercent) * 255);
                oRetColor = Color.FromRgb(0xFF, 0x00, (byte)Blue);
            }

            return oRetColor;
        }
    }
}
