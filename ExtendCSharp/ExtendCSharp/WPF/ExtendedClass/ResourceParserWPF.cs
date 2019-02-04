using ExtendCSharp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ExtendCSharp.WPF.ExtendedClass
{
    public class ResourceBitmapImageParse : ResourceParser
    {
        public ResourceBitmapImageParse() : base(typeof(BitmapImage))
        {

        }
        protected ResourceBitmapImageParse(Type resourceType) : base(resourceType)
        {
        }

        public override object Parse(Stream s)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = s;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
