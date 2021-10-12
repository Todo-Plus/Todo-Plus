using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TodoListCSharp.utils {
    public class Generator {
        public static Grid TabViewGridGenerate(string title, Color color) {
            Grid grid = new Grid();
            grid.Margin = new Thickness(5);
            
            Border border = new Border();
            border.Background = new SolidColorBrush(color);
            border.CornerRadius = new CornerRadius(5);
            
            Label label = new Label();
            label.Content = title;
            label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            grid.Children.Add(border);
            grid.Children.Add(label);

            return grid;
        }
    }
}