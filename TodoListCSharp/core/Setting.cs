using System;
using System.Drawing;

namespace TodoListCSharp.core {
    [Serializable]
    public class Setting {
        // Appearance
        private int Alpha;
        private int Fontsize;
        private string Fontfamily;
        private Color BackgroundColor;

        public Setting() {
            // default setting
            Alpha = 100;
            Fontsize = 16;
            Fontfamily = "Inter";
            BackgroundColor = Color.White;
        }
    }
}