using System;
using System.Drawing;
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
        private String textCaption = "";
        public String TextCaption {
            get => textCaption;
            set
            {
                textCaption = value;
                if(status == TextCaptionStatus.visible)
                {
                    base.Text = value;
                }
            }
        }
        public Color CaptionColor { get; set; } = Color.Gray;
        private Color NormalTextColor { get; set; } = Color.Black;

        public override Color ForeColor
        {
            get => base.ForeColor;
        }
       

        public bool AutoScroll { get; set; } = false;

        protected TextCaptionStatus status;

        public event ValidationTextDelegate ValidationText;

        public TextBoxPlus()
        {
            KeyDown += (object sender, KeyEventArgs e) =>
            {
                KeyDownPlus?.Invoke(this, e);
            };
            KeyDownPlus += Ctrl_A_Checker;
            KeyDownPlus += EnterSendChecker;
            GotFocus += TextBoxPlus_GotFocus;
            LostFocus += TextBoxPlus_LostFocus;
            NormalTextColor = ForeColor;

            PrintCaptionText();
        }

       

     

        private void TextBoxPlus_GotFocus(object sender, EventArgs e)
        {
            if (ReadOnly)
                return;

            if (status == TextCaptionStatus.visible)
            {
                base.Text = "";
                ForeColor = NormalTextColor;
                status = TextCaptionStatus.invisible;
            }
        }
        private void TextBoxPlus_LostFocus(object sender, EventArgs e)
        {
            if (ReadOnly)
                return;

            StartTextValidation();
        }

        public void StartTextValidation()
        {
            status = TextCaptionStatus.invisible;   //qualsiasi cosa ci sia scritta, la valuto come testo scritto
            bool? Valid = ValidationText?.Invoke(base.Text);
            if(Valid.HasValue)  //se ho ricevuto un valore
            {
                if (Valid.Value)    //postivo
                {
                    //lascio il testo corrente e lo status su invisible
                }
                else
                {
                    PrintCaptionText();
                }
            }
            else    //non ho ricevuto risposta
            {
                if (base.Text == "")
                {
                    PrintCaptionText();
                }
                else
                {
                    //lascio il testo corrente e lo status su invisible
                }
            }


        }
        
        private void PrintCaptionText()
        {
            //cancello il testo corrente e visualizzo la caption
            ForeColor = CaptionColor;
            base.Text = TextCaption;

            //imposto di visualizzare la caption
            status = TextCaptionStatus.visible;
        }
        


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
                if (status == TextCaptionStatus.visible)
                    return "";
                return base.Text;
            }
        }

        public new void AppendText(String s)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.AppendText(s); });
            else
            {
                base.Text = base.Text + s;

                StartTextValidation();

                if (AutoScroll)
                {
                    this.SelectionStart = this.Text.Length;
                    this.ScrollToCaret();
                }

            }

        }
        public void AppendLine(String line)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.AppendLine(line); });
            else
            {
                base.Text = base.Text + line.RemoveRight("\r", "\n", "\r\n") + "\r\n";

                StartTextValidation();


                if (AutoScroll)
                {
                    this.SelectionStart = this.Text.Length;
                    this.ScrollToCaret();
                }
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

                StartTextValidation();


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

    public enum TextCaptionStatus
    {
        visible,
        invisible
    }

    public delegate bool? ValidationTextDelegate(string textToValidate);
}
