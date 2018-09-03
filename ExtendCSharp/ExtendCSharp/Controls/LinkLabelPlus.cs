using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class LinkLabelPlus : LinkLabel
    {
        private object _Object = default(object);
        public object LinkObject
        {
            get
            {
                return _Object;
            }

            set
            {
                _Object = value;
            }
        }




        public LinkLabelPlus()
        {
            InitializeComponent();
        }

        public LinkLabelPlus(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
