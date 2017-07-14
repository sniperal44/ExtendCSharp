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
        public TextBoxPlus():base()
        {
            KeyDownPlus -= EnterSendChecker;
            KeyDownPlus += EnterSendCheckerDefault;

            
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
        private void EnterSendCheckerDefault(TextBoxPlus<String> sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                bool Suppress = false;
                OnEnterSend?.Invoke(this, e, ref Suppress);
                if (Suppress)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
        }


        public delegate void EnterSendEventHandlerDefault(TextBoxPlus sender, KeyEventArgs e, ref bool Suppress);
        public new event EnterSendEventHandlerDefault OnEnterSend;



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
            KeyDownPlus += EnterSendChecker;
        }
        public bool AutoScroll { get; set; } = false;


        protected void EnterSendChecker(TextBoxPlus<T> sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.Enter)
            {
                bool Suppress = false;
                OnEnterSend?.Invoke(this,e,ref Suppress);
                if (Suppress)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
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
        public void AppendLine(String line)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.AppendLine(line); });
            else
                base.Text = base.Text + line.RemoveRight("\r", "\n", "\r\n") + "\r\n";

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

        /// <summary>
        /// Evento generato quando viene premuto in tasto Enter
        /// </summary>
        /// <param name="sender"></param>
        public delegate void EnterSendEventHandler(TextBoxPlus<T> sender, KeyEventArgs e, ref bool Suppress);
        public event EnterSendEventHandler OnEnterSend;

    }

}
