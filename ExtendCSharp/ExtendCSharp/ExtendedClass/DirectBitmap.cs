using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
            //TODO: controllare
            if(bits.Length<width*height)
            {
                byte[] tmp = new byte[width * height];  //evito che la bitmap vada a puntare in uno spazio di memoria vuoto ( allungo i miei bit ) 
                for (int i = 0; i < bits.Length; i++)
                    tmp[i] = bits[i];
            }
            Width = width;
            Height = height;
            Bits = bits;
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
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
