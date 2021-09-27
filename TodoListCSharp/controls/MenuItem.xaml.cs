using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoListCSharp.controls
{
    /// <summary>
    /// MenuItem 菜单项
    /// params：
    /// @IconSource： ImageSource
    /// @Text： string
    /// @FontSize: int
    /// @FontColor: Color
    /// 
    /// functions：
    /// @ Click： function
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public MenuItem()
        {
            InitializeComponent();
        }
    }
}
