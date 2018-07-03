using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class PanelPlus : Panel
    {
        public new bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                base.DoubleBuffered = value;
            }

        }



        public PanelPlus() : base()
        {
            InitializeComponent();
        }
        public PanelPlus(IContainer container):base()
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Area di disegno interno (Width), escluso il bordo
        /// </summary>
        public int InternalWidth
        {
            get
            {
                int DrawWidth = this.Width;

                if (BorderStyle == BorderStyle.FixedSingle)
                    DrawWidth -= 2;
                else if (BorderStyle == BorderStyle.Fixed3D)
                    DrawWidth -= 4;

                return DrawWidth;
            }
        }
        /// <summary>
        /// Area di disegno interno (Height), escluso il bordo
        /// </summary>
        public int InternalHeight
        {
            get
            {
                int DrawHeight = this.Height;

                if (BorderStyle == BorderStyle.FixedSingle)
                    DrawHeight -= 2;
                else if (BorderStyle == BorderStyle.Fixed3D)
                    DrawHeight -= 4;

                return DrawHeight;
            }
        }

        public Rectangle GetBackgroundRect()
        {
            if (BackgroundImage == null)
                return Rectangle.Empty;

            if (BackgroundImageLayout == ImageLayout.None || BackgroundImageLayout == ImageLayout.Tile)
            {
                return new Rectangle(new Point(0, 0), BackgroundImage.Size);
            }

            if (BackgroundImageLayout == ImageLayout.Stretch)
            {
                return new Rectangle(new Point(0, 0), new Size(InternalWidth,InternalHeight));
            }

            if (BackgroundImageLayout == ImageLayout.Center)
            {
                int X=0, Y=0;
                if (BackgroundImage.Width < InternalWidth)
                    X = (this.Width - BackgroundImage.Width) / 2;

                if (BackgroundImage.Height < InternalHeight)
                    Y = (this.Height - BackgroundImage.Height) / 2;

                return new Rectangle(new Point(X, Y), BackgroundImage.Size);
            }

            if (BackgroundImageLayout == ImageLayout.Zoom)
            {
                //TODO: controllo se usando l'InternalWidth ( senza il bordo ) tutto funziona

                int X, Y,Width, Height;

                double DrawWidth = InternalWidth;
                double DrawHeight = InternalHeight;


                double ratioX = DrawWidth / BackgroundImage.Width;
                double ratioY = DrawHeight / BackgroundImage.Height;
                double ratio = Math.Min(ratioX, ratioY);

                Width = (int)(BackgroundImage.Width * ratio);
                Height = (int)(BackgroundImage.Height * ratio);


                X = (int)((DrawWidth - Width) / 2);
                Y = (int)((DrawHeight - Height) / 2);



                return new Rectangle((int)X, (int)Y, Width, Height);

            }
            
            return Rectangle.Empty;
        }


        public double GetBackgroundWidthProportion()
        {
            if (BackgroundImage == null)
                return 0;

            if (BackgroundImageLayout == ImageLayout.None || BackgroundImageLayout == ImageLayout.Tile)
            {
                return 1;
            }

            if (BackgroundImageLayout == ImageLayout.Stretch)
            {
                return InternalWidth / BackgroundImage.Width;
            }

            if (BackgroundImageLayout == ImageLayout.Center)
            {
                return 1;
            }

            if (BackgroundImageLayout == ImageLayout.Zoom)
            {
                double DrawWidth = InternalWidth;
                double DrawHeight = InternalHeight;


                double ratioX = DrawWidth / BackgroundImage.Width;
                double ratioY = DrawHeight / BackgroundImage.Height;
                double ratio = Math.Min(ratioX, ratioY);

                return ratio;  
            }

            return 0;
        }
        public double GetBackgroundHeightProportion()
        {
            if (BackgroundImage == null)
                return 0;

            if (BackgroundImageLayout == ImageLayout.None || BackgroundImageLayout == ImageLayout.Tile)
            {
                return 1;
            }

            if (BackgroundImageLayout == ImageLayout.Stretch)
            {
                return InternalHeight / BackgroundImage.Height;
            }

            if (BackgroundImageLayout == ImageLayout.Center)
            {
                return 1;
            }

            if (BackgroundImageLayout == ImageLayout.Zoom)
            {
                double DrawWidth = InternalWidth;
                double DrawHeight = InternalHeight;


                double ratioX = DrawWidth / BackgroundImage.Width;
                double ratioY = DrawHeight / BackgroundImage.Height;
                double ratio = Math.Min(ratioX, ratioY);

                return ratio;
            }

            return 0;
        }



    }
}
