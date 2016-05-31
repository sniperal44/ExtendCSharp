using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
    public partial class SliderForm : Form
    {
        SlideFormButton Buttons = 0;
        int MarginBottom = 10;
        int Distanza = 50;

        Dictionary<SlideFormButton, Button> DictButton;
        public SliderForm(SlideFormButton Buttons)
        {
            InitializeComponent();
            this.Buttons = Buttons;

            DictButton = new Dictionary<SlideFormButton, Button>();
            InitializeDictionaryButton();

            foreach (KeyValuePair<SlideFormButton,Button> kv in DictButton)
            {
                if (Buttons.HasFlag(kv.Key))
                {
                    kv.Value.Visible = true;
                }
            }
        }
        private void InitializeDictionaryButton()
        {
            DictButton.Add(SlideFormButton.Top, button_top);
            DictButton.Add(SlideFormButton.Bottom, button_bottom);
            DictButton.Add(SlideFormButton.Left, button_left);
            DictButton.Add(SlideFormButton.Right, button_right);
        }
        private void SliderPanel_SizeChanged(object sender, EventArgs e)
        {
            button_bottom.Location = new Point(Width / 2 - button_bottom.Width / 2, ClientRectangle.Height - MarginBottom - button_bottom.Height);
            button_top.Location = new Point(Width / 2 - button_bottom.Width / 2, ClientRectangle.Height - MarginBottom - button_bottom.Height- Distanza);
            button_right.Location = new Point(Width / 2 - button_bottom.Width / 2+ Distanza, ClientRectangle.Height - MarginBottom - button_bottom.Height- Distanza/2);
            button_left.Location = new Point(Width / 2 - button_bottom.Width / 2- Distanza, ClientRectangle.Height - MarginBottom - button_bottom.Height-Distanza/2);

            Panel_Container.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - MarginBottom - button_bottom.Height - Distanza - Distanza / 2);
        }

        private void SliderForm_Load(object sender, EventArgs e)
        {
            
            SliderPanel_SizeChanged(this,null);
        }
    }


    public enum SlideFormButton
    {
        nul=0,
        Top=1,
        Right=2,
        Bottom=4,
        Left=8,
    }
}
