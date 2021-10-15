using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TodoListCSharp.converter {
    public class MediaColorToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            SolidColorBrush brush = (SolidColorBrush)value;
            if (brush != null) return brush.Color;
            return null;
        }
    }
}