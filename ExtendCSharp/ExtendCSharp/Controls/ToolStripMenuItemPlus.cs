using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class ToolStripMenuItemPlus: ToolStripMenuItem
    {
        public ToolStripMenuItemPlus()
        {
            TextObject = "";
        }
        public ToolStripMenuItemPlus(object o)
        {
            TextObject = o;
        }

        private object _TextObject = null;


        public object TextObject
        {
            get
            {
                return _TextObject;
            }

            set
            {
                _TextObject = value;
                if (value == null)
                    base.Text = null;
                else
                    base.Text = value.ToString();

            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                TextObject = value;
            }
        }
    }
}
