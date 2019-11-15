using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Classes
{
    public class JPG
    {
        public byte[] data { get; set; }

        public JPG(Bitmap bitmap)
        {
            using (MemoryStream ImgTmp = new MemoryStream())
            {
                bitmap.Save(ImgTmp, ImageFormat.Jpeg);
                data = ImgTmp.ToArray();
            }
        }
        public JPG(byte[] data)
        {
            this.data = data;

        }

        public Bitmap ToBitmap()
        {
            using (MemoryStream img = new MemoryStream())
            {
                img.Write(this.data, 0, this.data.Length);
                Bitmap b = new Bitmap(img);
                return b;
            }
        }
    }
}
