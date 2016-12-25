using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class TextBoxPlus : TextBoxPlus<String>
    {      
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
                KeyDownPlus?.Invoke(this, e);
            };
            KeyDownPlus += Ctrl_A_Checker;

        }
        public bool AutoScroll { get; set; } = false;

        private bool _EnterSend = false;
        public bool EnterSend
        {
            get
            {
                return _EnterSend;
            }
            set
            {
                _EnterSend = value;
                if (value)
                    KeyDownPlus += EnterSendChecker;
                else
                    KeyDownPlus -= EnterSendChecker;
            }
        }




        private void EnterSendChecker(TextBoxPlus<T> sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                OnEnterSend(this);
            }
        }

        private void Ctrl_A_Checker(TextBoxPlus<T> sender, KeyEventArgs e)
        {
            
            if (e.KeyData == (Keys.A | Keys.Control))
            {
                sender.SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
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
                SetText(value);

            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
        }

        public new void AppendText(String s)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.AppendText(s); });
            else
                base.Text = base.Text + s;

            if (AutoScroll)
            {
                this.SelectionStart = this.Text.Length;
                this.ScrollToCaret();
            }
        }
        private void SetText(object s)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.SetText(s); });
            else
            {
                if (s == null)
                    base.Text = null;
                else
                    base.Text = s.ToString();

                if (AutoScroll)
                {
                    SelectionStart = base.Text.Length;
                    ScrollToCaret();
                }
            }

            
        }

        public delegate void KeyPlusEventHandler(TextBoxPlus<T> sender, KeyEventArgs e);
        public event KeyPlusEventHandler KeyDownPlus;

        public delegate void EnterSendEventHandler(TextBoxPlus<T> sender);
        public event EnterSendEventHandler OnEnterSend;

    }

}
