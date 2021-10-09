using System;
using System.Windows.Media;

namespace TodoListCSharp.core {
    // Setting类，单例模式，beta版本，后续进行设计
    [Serializable]
    public class Setting {
        // Appearance
        public int Alpha { get; set; }
        public int Fontsize { get; set; }
        public string FontFamily { get; set; }
        public Color BackgroundColor { get; set; }

        public Setting() {
            // default setting
            Alpha = 100;
            Fontsize = 16;
            FontFamily = "Inter";
            BackgroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
        }
    }
}