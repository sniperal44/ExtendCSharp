using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

[assembly: InternalsVisibleTo("Extension")]
namespace ExtendCSharp.Controls
{

    /// <summary>
    /// PictureBoxPlus implementa molti metodi per la gestione dei movimenti/animazioni/collisioni tra PictureBox
    /// Per usarle al meglio conviene non usare il campo BackgroundImage ( usare il campo Image ) 
    /// </summary>
    public class PictureBoxPlus : PictureBox
    {
        
        internal bool _disableBitmapCreation = false;

        public PictureBoxPlus()
        {

        }
        public PictureBoxPlus(Bitmap b)
        {
            BackgroundImage = b;
        }



        
        private Bitmap _ForegroundOrigin, _Foreground, _UnitedBitmap;
       
        public Bitmap ForegroundOrigin
        {
            get
            {
                return _ForegroundOrigin;
            }
        }
        public Bitmap Foreground
        {
            get
            {
                return _Foreground;
            }
        }
        public Bitmap UnitedBitmap
        {
            get
            {
                return _UnitedBitmap;
            }
        }





        public new Image Image
        {
            get
            {
                return base.Image;
            }

            set
            {
                base.Image = value;
                if (_disableBitmapCreation)
                    return;

                if (value == null)
                {
                    _ForegroundOrigin = null;
                    _Foreground = null;
                }
                else
                {
                    _ForegroundOrigin = new Bitmap(value);
                    RicalcolaForeground();
                }

                RicalcolaUnitedBitmap();
            }
        }
        public new Size Size
        {
            get
            {
                return base.Size;
            }

            set
            {
                base.Size = value;
                if (_disableBitmapCreation || value == null)
                    return;

                RicalcolaForeground();
                RicalcolaUnitedBitmap();
            }
        }
        public new Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }

            set
            {
                base.Bounds = value;
                if (_disableBitmapCreation || value == null)
                    return;

                RicalcolaForeground();
                RicalcolaUnitedBitmap();
            }
        }
        public new PictureBoxSizeMode SizeMode
        {
            get
            {
                return base.SizeMode;
            }

            set
            {
                base.SizeMode = value;
                if (_disableBitmapCreation)
                    return;

                RicalcolaForeground();
                RicalcolaUnitedBitmap();
            }
        }
        public new ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }

            set
            {
                base.BackgroundImageLayout = value;
                if (_disableBitmapCreation)
                    return;

                RicalcolaForeground();
                RicalcolaUnitedBitmap();
            }
        }
        
    
        private void RicalcolaForeground()
        {
            _Foreground = new Bitmap(Size.Width,Size.Height);
            DrawOverGraphics(Graphics.FromImage(_Foreground),0,0);
        }
        private void RicalcolaUnitedBitmap()
        {
            _UnitedBitmap = CreateBitmap();
        }

      
        public bool PixelCollision(PictureBoxPlus pict2)
        {
            if (!base.Bounds.IntersectsWith(pict2.Bounds))
                return false;

            Rectangle ra = this.Bounds;
            Rectangle rb = pict2.Bounds;

            Bitmap Temp1 = _Foreground;
            Bitmap Temp2 = pict2._Foreground;


            // Calculate the intersecting rectangle
            int x1 = Math.Max(ra.X, rb.X);
            int x2 = Math.Min(ra.X + ra.Width, rb.X + rb.Width);

            int y1 = Math.Max(ra.Y, rb.Y);
            int y2 = Math.Min(ra.Y + ra.Height, rb.Y + rb.Height);

            int width = x2 - x1;
            int heigth = y2 - y1;






            unsafe
            {
                BitmapData aData = null, bData = null;
                try
                {

                    aData = Temp1.LockBits(new Rectangle(x1 - ra.X, y1 - ra.Y, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    bData = Temp2.LockBits(new Rectangle(x1 - rb.X, y1 - rb.Y, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


                    byte* aPointer = (byte*)aData.Scan0;
                    byte* bPointer = (byte*)bData.Scan0;



                    var bppa = aData.Stride / Temp1.Width;
                    var bppb = bData.Stride / Temp2.Width;



                    for (var y = 0; y < aData.Height; y++)
                    {
                        var rowa = aPointer + (y * aData.Stride);
                        var rowb = bPointer + (y * bData.Stride);

                        for (var x = 0; x < aData.Width; x++)
                        {
                            byte* pixelA = rowa + x * bppa;
                            byte* pixelB = rowb + x * bppa;

                            if (pixelA[3] != 0 && pixelB[3] != 0)
                            {
                                return true;
                            }
                        }
                    }


                }
                catch (Exception)
                {

                }
                finally
                {
                    if (aData != null)
                        Temp1.UnlockBits(aData);
                    if (bData != null)
                        Temp2.UnlockBits(bData);
                }

            }



            return false;
        }

        public PictureBoxPlus Clone()
        {

            PictureBoxPlus temp = new PictureBoxPlus()
            {
                _disableBitmapCreation = true,
                BackgroundImage = BackgroundImage,
                Image = Image,
                BackgroundImageLayout = BackgroundImageLayout,
                SizeMode = SizeMode,
                Size = Size,
                Bounds = Bounds
            };
            foreach (Bitmap b in ListaAnimazioni)
                temp.ListaAnimazioni.Add((Bitmap)b.Clone());

            temp.Current = Current;
            temp._disableBitmapCreation = false;
            return temp;
        }



        public int X
        {
            get
            {
                return base.Location.X;
            }
            set
            {
                Location = new Point(value, Location.Y);
            }
        }
        public int Y
        {
            get
            {
                return base.Location.Y;
            }
            set
            {
                Location = new Point(Location.X, value);
            }
        }



        int Current = 0;
        public List<Bitmap> ListaAnimazioni = new List<Bitmap>();
        

        public void AddAnimazione(Bitmap b)
        {
            ListaAnimazioni.Add(b);
        }
        public void AddAnimazioneFromGif(Image gif)
        {
            FrameDimension dim = new FrameDimension(gif.FrameDimensionsList[0]);
            int frames = gif.GetFrameCount(dim);

            for (int i = 0; i < frames; i++)
            {
                gif.SelectActiveFrame(dim, i);
                Bitmap t = new Bitmap(gif.Width, gif.Height);
                Graphics.FromImage(t).DrawImage(gif,0,0);
                ListaAnimazioni.Add(t);
            }

        }



        public void NextAnimazione()
        {
            if (ListaAnimazioni.Count == 0)
                return;

            Current++;
            if (Current > ListaAnimazioni.Count - 1)
                Current = 0;

            Image = ListaAnimazioni[Current];
        }


        public void SettaAnimazione(int i)
        {
            if (i > ListaAnimazioni.Count)
                return;

            Image = ListaAnimazioni[i];
        }
        public void ClearAnimazioni()
        {
            ListaAnimazioni.Clear();
        }




        public int XCenter
        {
            get { return X+Width/2; }
            set
            {
                int new_x = value - Width / 2;
                if (X == new_x)
                    return;

                X = new_x;
            }
        }
        public int YCenter
        {
            get { return Y + Height / 2; }
            set
            {
                int new_y = value - Height / 2;
                if (Y == new_y)
                    return;

                Y = new_y;
            }
        }
        

        public void DrawOverGraphics(Graphics g)
        {
            DrawOverGraphics(g, X, Y);
        }
        public void DrawOverGraphics(Graphics g, int x, int y)
        {
            g.TranslateTransform(x, y);
            base.OnPaint(new PaintEventArgs(g, new Rectangle(0, 0, Width, Height)));
            g.TranslateTransform(-x, -y);
        }



        public Bitmap CreateBitmap()
        {
            Bitmap b = new Bitmap(Width, Height);
            DrawToBitmap(b, ClientRectangle);
            return b;
        }

        /*protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            
        }*/
    }
}
