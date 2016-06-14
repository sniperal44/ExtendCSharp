using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        SliderNode CurrentNode = null;
        public SliderForm(SliderNode EntryNode)
        {
            InitializeComponent();

            DictButton = new Dictionary<SlideFormButton, Button>();
            InitializeDictionaryButton();

            foreach (KeyValuePair<SlideFormButton,Button> kv in DictButton)
            {
                kv.Value.Click += ButtonClick;
            }

            CurrentNode = EntryNode;
            LoadCurrentNode();

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if(!DictButton.ContainsValue(sender as Button))
                return;

            SlideFormButton btn = DictButton.First((x) => x.Value == sender as Button).Key;
            
            CanGoReturn r = CurrentNode.CanGo(btn); 
            if(r==false)
            {
                MessageBox.Show(r.Message);
                return;
            }

            
            CurrentNode=CurrentNode.GetNext(btn);
            LoadCurrentNode();
        }

        private void InitializeDictionaryButton()
        {
            DictButton.Add(SlideFormButton.Top, button_top);
            button_top.Text = "Su";
            DictButton.Add(SlideFormButton.Bottom, button_bottom);
            button_bottom.Text = "Giu";
            DictButton.Add(SlideFormButton.Left, button_left);
            button_left.Text = "Precedente";
            DictButton.Add(SlideFormButton.Right, button_right);
            button_right.Text = "Successivo";
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

        public void LoadCurrentNode()
        {
            if (Panel_Container.Contains(CurrentNode.panel))
                return;

            
            if (CurrentNode == null || CurrentNode.panel==null)
                return;

            CurrentNode.panel.Location = new Point(Panel_Container.Width, 0);
            CurrentNode.panel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            CurrentNode.panel.Size = new Size(Panel_Container.Width, Panel_Container.Height);
            Panel_Container.Controls.Add(CurrentNode.panel);

           
            
           
            new Thread(() =>
            {
                SliderPanel p = CurrentNode.panel;
                while (CurrentNode.panel.Location.X > 0)
                {
                    foreach (Control c in Panel_Container.Controls)
                        c.SetLocationInvoke(c.Location.X - 2, c.Location.Y);
                    Thread.Sleep(1);
                }
                CurrentNode.panel.SetLocationInvoke(0, CurrentNode.panel.Location.Y);

                int i = 0, count = Panel_Container.Controls.Count;
                while(count>0)
                {
                    if (Panel_Container.Controls[i] == CurrentNode.panel)
                        i++;
                    else
                        Panel_Container.Controls.RemoveAt(i);

                    count--;
                }



            }).Start();
           
            Buttons = CurrentNode.GetButtons();
            foreach (KeyValuePair<SlideFormButton, Button> kv in DictButton)
            {
                if (Buttons.HasFlag(kv.Key))
                {
                    kv.Value.Visible = true;
                    kv.Value.Enabled = true;
                }
                else
                {
                    kv.Value.Visible = false;
                    kv.Value.Enabled = false;
                }
            }
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
