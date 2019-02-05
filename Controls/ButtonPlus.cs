using System.Windows.Forms;

namespace ExtendCSharp.Controls
{

    public class ButtonPlus : ButtonPlus<object>
    {
        private object _TextObject = default(object);


        public new object TextObject
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
    }
    public class ButtonPlus<T>:Button
    {
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
    }
}
