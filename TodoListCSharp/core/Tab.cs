using System;
using System.Windows.Media;
using TodoListCSharp.utils;

namespace TodoListCSharp.core {
    [Serializable]
    public class Tab {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Color { set; get; }

        public Tab(int _id, string _name, Color _color) {
            Id = _id;
            Name = _name;
            Color = Utils.MediaColorToHex(_color);
        }

        public void SetColor(Color color) {
            Color = Utils.MediaColorToHex(color);
        }

        public Color GetColor() {
            return Utils.HexToMediaColor(Color);
        }
    }
}