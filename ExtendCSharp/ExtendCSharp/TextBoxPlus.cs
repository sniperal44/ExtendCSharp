using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
    public class TextBoxPlus : TextBox
    {
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
    public class TextBoxPlus<T> : TextBox
    {
        public TextBoxPlus()
        {
            KeyDown += (object sender, KeyEventArgs e) =>
            {
                KeyDownPlus(this, e);
            };
        }



        private T _TextObject = default(T);

        public T TextObject
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
        }

        public delegate void KeyPlusEventHandler(TextBoxPlus<T> sender, KeyEventArgs e);
        public event KeyPlusEventHandler KeyDownPlus;
    }

}
