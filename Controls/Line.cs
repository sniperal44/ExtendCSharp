using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class Line : UserControl
    {
        public Color LineColor { get; set; } = Color.Black;
        public LineOrientationEnum LineOrientation { get; set; } = LineOrientationEnum.Horizontal;

        public Line()
        {
            InitializeComponent();
            BackColor = LineColor;
            if (LineOrientation == LineOrientationEnum.Horizontal)
            {
                Height = 1;
                MaximumSize = new Size(9999, 1);
            }
            else
            {
                Width = 1;
                MaximumSize = new Size(1, 9999);
            }
        }
    }

    public enum LineOrientationEnum
    {
        Horizontal,
        Vertical
       
    }

}
