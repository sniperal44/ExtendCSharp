using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{
    public static class FileInfoExtension
    {
        public static string GetFileNameWithoutExtension(this FileInfo self)
        {
            return Path.GetFileNameWithoutExtension(self.Name);
        }
    }
}
