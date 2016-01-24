using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public static class SystemService
    {
        public static bool  CreateFolderSecure(String Path)
        {
            if(Directory.Exists(Path))
                return true;
            return Directory.CreateDirectory(Path).Exists; 
        }

        public static String GetParent(String s)
        {
            return Directory.GetParent(s).FullName;
        }
       
        public static String ChangeExtension(String Path,String Ext)
        {
            int sindex = Path.LastIndexOf('.');
            if (sindex == -1)
                return Path;
            return Path.Remove(sindex) + '.' + Ext.Trim(' ', '.').ToLower();
        }

        public static String Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public static void CopySecure(String Source,String Dest, bool Override=true,CopyProgressChangedDelegate OnProgressChanged =null, CopyCompleteDelegate OnComplete=null)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool cancelFlag = false;

            using (FileStream source = new FileStream(Source, FileMode.Open, FileAccess.Read))
            {
                long fileLength = source.Length;
                if (File.Exists(Dest))
                {
                    if (Override)
                        File.Delete(Dest);
                    else
                    {
                        if (OnComplete != null)
                            OnComplete(false);
                        return;
                    }
   
                }

                SystemService.CreateFolderSecure(SystemService.GetParent(Dest));
                using (FileStream dest = new FileStream(Dest, FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double persentage = (double)totalBytes * 100.0 / fileLength;

                        dest.Write(buffer, 0, currentBlockSize);

                        if (OnProgressChanged != null)
                        {
                            cancelFlag = false;
                            OnProgressChanged(persentage, ref cancelFlag);
                            if (cancelFlag)
                                break;
                            
                        }
                    }
                }
                if (cancelFlag)
                    if (File.Exists(Dest))
                        File.Delete(Dest);

            }
            if(OnComplete!=null)
                OnComplete(true);
        }

        public static bool FileExist(String Path)
        {
            return File.Exists(Path);
        }
        public static bool DirectoryExist(String Path)
        {
            return Directory.Exists(Path);
        }
        public static bool Exist(String Path)
        {
            return FileExist(Path) || DirectoryExist(Path);
        }

        public static String GetMD5(String Path)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(Path))
                    {
                        return md5.ComputeHash(stream).ToHex(true);
                    }

                }
            }
            catch(Exception)
            {
                return "";
            }
        }

        public delegate void CopyProgressChangedDelegate(double persentage,ref bool cancelFlag);
        public delegate void CopyCompleteDelegate(bool copiato);

    }
}
