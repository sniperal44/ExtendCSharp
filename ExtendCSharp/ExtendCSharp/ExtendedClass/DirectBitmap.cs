using ExtendCSharp.Enums;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ExtendCSharp.ExtendedClass
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public byte[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new byte[width * height * 4];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }
        public DirectBitmap(Image source)
        {
            Width = source.Width;
            Height = source.Height;
            Bits = new byte[Width * Height * 4];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
            using (var g = Graphics.FromImage(Bitmap))
            {
                g.DrawImage(source, 0, 0);
            }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits">in formato ARGB</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public DirectBitmap(byte[] bits, int width, int height)
        {
            if(bits.Length<width*height*4)
            {
                byte[] tmp = new byte[width * height*4];  //evito che la bitmap vada a puntare in uno spazio di memoria vuoto ( allungo i miei bit ) 
                for (int i = 0; i < bits.Length; i++)
                    tmp[i] = bits[i];

                bits = tmp;
            }
            Width = width;
            Height = height;
            Bits = bits;
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }


        public DirectBitmap( BitmapImage source)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream))
                {
                    Width = bitmap.Width;
                    Height = bitmap.Height;
                    Bits = new byte[Width * Height * 4];
                    BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                    Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
                    using (var g = Graphics.FromImage(Bitmap))
                    {
                        g.DrawImage(bitmap, 0, 0);
                    }
                }
            }




            /*
            
            //OLD METHOD! 
            
            Height = source.PixelHeight;
            Width = source.PixelWidth;
            int nStride = (Width * source.Format.BitsPerPixel + 7) / 8;
            Bits = new byte[Height * nStride];
            source.CopyPixels(Bits, nStride, 0);

            //Aggiusto le posizioni dei pixel
            for (int i = 0; i < Bits.Length; i += 4)
            {
                //Inverto A con B
                byte A = Bits[i + 3];
                Bits[i + 3] = Bits[i];
                Bits[i] = A;


                //Inverto R con G
                byte R = Bits[i + 2];
                Bits[i + 2] = Bits[i + 1];          //Metto la B in pos 4
                Bits[i + 1] = R;


                //BGRA
                //ARGB

                //A
                //GRGB
            }

            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());

            */
        }

        public DirectBitmap(System.Windows.Media.ImageSource image):this((BitmapImage) image)
        {
            
        }

        
        public byte this[int x, int y,RGBA tono]
        {
            get
            {
                int index = (Width * y + x)*4;
                return Bits[index + (int)tono];
            }
            set
            {
                int index = (Width * y + x)*4;
                Bits[index + (int)tono] = value;
            }
        }

        public Color this[int x,int y] {
            get
            {
                int index = Width * y + x;
                return this[index];
            }
            set
            {
                int index = Width * y + x;
                this[index] = value;
            }
        }
        public Color this[int key]
        {
            get
            {
                key *= 4;
                if (key > Bits.Length)
                    throw new IndexOutOfRangeException();
                else
                    return Color.FromArgb(Bits[key], Bits[key + 1], Bits[key + 2], Bits[key + 3]);
            }
            set
            {
                key *= 4;
                if (key > Bits.Length)
                    throw new IndexOutOfRangeException();
                else
                {
                    Bits[key] = value.A;
                    Bits[key + 1] = value.R;
                    Bits[key + 2] = value.G;
                    Bits[key + 3] = value.B;
                }
                    
            }
        }


        public int getA(int x, int y)
        {
            int index = (Width * y + x)*4;
            if (index > Bits.Length)
                throw new IndexOutOfRangeException();
            return Bits[index];
        }
        public int getR(int x, int y)
        {
            int index = (Width * y + x) * 4;
            if (index > Bits.Length)
                throw new IndexOutOfRangeException();
            return Bits[index+1];
        }
        public int getG(int x, int y)
        {
            int index = (Width * y + x) * 4;
            if (index > Bits.Length)
                throw new IndexOutOfRangeException();
            return Bits[index+2];
        }
        public int getB(int x, int y)
        {
            int index = (Width * y + x) * 4;
            if (index> Bits.Length)
                throw new IndexOutOfRangeException();
            return Bits[index+3];
        }

       
        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }

    
}
