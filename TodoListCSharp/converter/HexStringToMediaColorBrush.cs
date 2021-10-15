using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoListCSharp.utils;

namespace TodoListCSharp.converter {
    public class HexStringToMediaColorBrush : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string sHexString = (string)value;
            return new SolidColorBrush(Utils.HexToMediaColor(sHexString));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            SolidColorBrush brush = (SolidColorBrush)value;
            return Utils.MediaColorToHex(brush.Color);
        }
    }
}