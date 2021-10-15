using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace TodoListCSharp.converter {
    public class HexStringToDrawColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string sHexString = (string)value;
            ColorConverter converter = new ColorConverter();
            return (Color)converter.ConvertFromString(sHexString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            Color color = (Color)value;
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}