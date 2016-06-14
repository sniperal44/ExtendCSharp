using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
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
