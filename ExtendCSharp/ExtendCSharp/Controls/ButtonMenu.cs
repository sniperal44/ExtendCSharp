using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class ButtonMenu : UserControl
    {
        //TODO: vedo se è possibile modificare un control di un altro control ( modificare il bottone dalla view del MainForm ) 
        String DownArrow = "▼";
        String UpArrow = "▲";

        ContextMenuStripStatus MenuContextStatus = ContextMenuStripStatus.Close;


        ContextMenuStrip _contextMenuStrip = null;
        bool ClosedLunchedByButton = false;

        public ContextMenuStrip MenuContext {
            get
            {
                return _contextMenuStrip;
            }
            set
            {
                _contextMenuStrip = value;
                if (_contextMenuStrip != null)
                {
                    _contextMenuStrip.Opened += ContextMenuStrip_Opened;
                    _contextMenuStrip.Closed += ContextMenuStrip_Closed;
                    _contextMenuStrip.Closing += _contextMenuStrip_Closing;
                }
            }

        }

        private void _contextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (!ClosedLunchedByButton && button_arrow.ClientRectangle.Contains(button_arrow.PointToClient(Control.MousePosition)))
            {
                e.Cancel = true;
            }
            ClosedLunchedByButton = false;

        }

        public ButtonMenu()
        {
            InitializeComponent();

            
        }

        

        private void ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            button_arrow.Text = UpArrow;
            MenuContextStatus = ContextMenuStripStatus.Open;
        }
        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            button_arrow.Text = DownArrow;
            MenuContextStatus = ContextMenuStripStatus.Close;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (_contextMenuStrip != null && _contextMenuStrip.Items.Count!=0)
            {
                if(MenuContextStatus==ContextMenuStripStatus.Close)
                {
                    _contextMenuStrip.Show(button.PointToScreen( button.ClientRectangle.GetLocation(ContentAlignment.BottomLeft)));
                }
                else if (MenuContextStatus == ContextMenuStripStatus.Open)
                {
                    ClosedLunchedByButton = true;
                    _contextMenuStrip.Close();
                }
                
            }
            
        }
    }
    enum ContextMenuStripStatus
    {
        Open,
        Close
    }
}
