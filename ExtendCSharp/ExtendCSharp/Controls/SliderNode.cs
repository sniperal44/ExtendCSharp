using ExtendCSharp.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Controls
{
    public class SliderNode
    {
        SliderPanel _panel;
        public SliderPanel panel
        {
            get
            {
                return _panel;
            }
            set
            {
                _panel = value;
            }
        }

        Dictionary<SlideFormButton, SliderNode> Nexts;
        public SliderNode()
        {
            panel = null;
            Nexts = new Dictionary<SlideFormButton, SliderNode>();
        }

        public void SetNext(SlideFormButton btn, SliderNode panel)
        {
            Nexts.Add(btn, panel);
        }
        public SliderNode GetNext(SlideFormButton btn)
        {
            if (Nexts.ContainsKey(btn))
                return Nexts[btn];
            return null;
        }
        public CanGoReturn CanGo(SlideFormButton dir)
        {
            return _panel.CanGo(dir);
        }


        public SlideFormButton GetButtons()
        {
            SlideFormButton t = SlideFormButton.nul;
            foreach (SlideFormButton b in Nexts.Select(x=>x.Key))
                t |= b;
            return t;
        }
    }
}
