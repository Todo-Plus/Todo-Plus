using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoListCSharp.utils;

namespace TodoListCSharp.converter {
    public class HexStringToMediaColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string sHexString = (string) value;
            return Utils.HexToMediaColor(sHexString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            Color color = (Color) value;
            return Utils.MediaColorToHex(color);
        }
    }
}