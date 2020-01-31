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

        public JPG(Bitmap bitmap):this(bitmap,100)
        {
            
        }

        /// <summary>
        /// Genera un JPG con qualità specificata (0-100)
        /// </summary>
        /// <param name="bitmap">Immagine da convertire</param>
        /// <param name="quality">Qualità del JPG ( da 0 a 100 )</param>
        public JPG(Bitmap bitmap,uint quality)
        {
            if (quality > 100)
                quality = 100;
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, quality);

            
            using (MemoryStream ImgTmp = new MemoryStream())
            {
                bitmap.Save(ImgTmp, jpgEncoder, encoderParameters);
                data = ImgTmp.ToArray();
            }
            encoderParameters.Param[0].Dispose();
            encoderParameters.Dispose();
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



        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }


        public static bool operator ==(JPG img1, JPG img2)
        {
            if (Object.ReferenceEquals(img1,null) && Object.ReferenceEquals(img2, null))
                return true;
            else if (Object.ReferenceEquals(img1, null) || Object.ReferenceEquals(img2, null))
                return false;

            else if (Enumerable.SequenceEqual(img1.data, img2.data))
                return true;
            return false;
        }
        public static bool operator !=(JPG img1, JPG img2)
        {
            return !(img1 == img2);
        }

    }
}
