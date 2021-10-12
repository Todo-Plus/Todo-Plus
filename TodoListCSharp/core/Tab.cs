using System.Windows.Media;

namespace TodoListCSharp.core {
    public class Tab {
        public int Id { set; get; }
        public string Name { set; get; }
        public Color Color { set; get; }

        public Tab(int _id, string _name, Color _color) {
            Id = _id;
            Name = _name;
            Color = _color;
        }
    }
}