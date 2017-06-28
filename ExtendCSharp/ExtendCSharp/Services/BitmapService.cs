using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExtendCSharp.Services
{
    public class BitmapService:IService
    {
        public Bitmap CreateBitmapFromFile(String Path)
        {
            SystemService ss = ServicesManager.GetOrSet(()=> { return new SystemService(); });
            if (!ss.FileExist(Path))
                throw new IOException("File " + Path + " non esistente");

            Bitmap b;
            using (var fs = new FileStream(Path, System.IO.FileMode.Open))
            {
                var bmp = new Bitmap(fs);
                b= (Bitmap)bmp.Clone();
                bmp.Dispose();
            }
            return b;

        }
    }
}
