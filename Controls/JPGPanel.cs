using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendCSharp.Classes;
using ExtendCSharp;


namespace ExtendCSharp.Controls
{
    public partial class JPGPanel : UserControl
    {
        //TODO: implemento Tiled, Stretched ecc
        public ImageLayout imageLayout = ImageLayout.Stretch;
        private JPG _jpg;
        public JPG jpg
        {
            get { return _jpg; } 
            set
            {
                _jpg = value;
                try
                {
                    this.Invalidate();
                }catch (Exception e)
                {

                }
            }
        }

        BufferedGraphics graphicsBuffer;        //Uso un buffer perchè a quanto pare è più veloce O.o

        public JPGPanel()
        {
            InitializeComponent();
        }
        protected override void OnPaintBackground(PaintEventArgs e) {/* just rely on the bitmap to fill the screen */}
        protected override void OnPaint(PaintEventArgs e)
        {
            //TODO: implemento tutti gli imageLayout
            
            if (_jpg == null)
                return;

            if(imageLayout==ImageLayout.Stretch)
            {
                using(Bitmap bitmap= _jpg.ToBitmap())
                {
                    Graphics g = graphicsBuffer.Graphics;

                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    g.DrawImage(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                    graphicsBuffer.Render(e.Graphics);
                }
               
            }
            //base.OnPaint(e);
        }

        private void JPGPanel_Resize(object sender, EventArgs e)
        {
            using (Graphics graphics = CreateGraphics())
            {
                graphicsBuffer = BufferedGraphicsManager.Current.Allocate(graphics, new Rectangle(0, 0, this.Width, this.Height));
            }
        }
    }
}
