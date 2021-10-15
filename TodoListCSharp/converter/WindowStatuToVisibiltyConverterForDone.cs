using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TodoListCSharp.core;

namespace TodoListCSharp.converter {
    public class WindowStatuToVisibiltyConverterForDone : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Constants.MainWindowStatu eStatu = (Constants.MainWindowStatu)value;
            if (eStatu == Constants.MainWindowStatu.TODO) {
                return Visibility.Visible;
            }
            else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            Visibility visibility = (Visibility)value;
            if (visibility == Visibility.Visible) {
                return Constants.MainWindowStatu.TODO;
            }
            else {
                return Constants.MainWindowStatu.DONE;
            }
        }
    }
}