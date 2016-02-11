using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExtendCSharp.Extension;


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
                            OnComplete(true);
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
        public static void GetMD5(String Path, MD5BlockTransformEventHandler OnMD5BlockTransform, MD5ComputeHashFinishEventHandler OnMD5ComputeHashFinish,bool Async=true)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(Path))
                    {      
                        md5.ComputeHashMultiBlockAsync(stream, OnMD5BlockTransform, OnMD5ComputeHashFinish, Async);
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnMD5ComputeHashFinish != null)
                    OnMD5ComputeHashFinish(null);
            }
        }



        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }


        #region Extension - File Association

        public static void SetAssociationFileExtention(string Extension, string ApplicationName, string AppPath, string FileDescription,string Icon=null)
        {
            Extension = "." + Extension.Trim('.').ToLowerInvariant();
            RegistryKey BaseKey;
            RegistryKey OpenMethod;
            RegistryKey Shell;

            BaseKey = Registry.ClassesRoot.CreateSubKey(Extension);
            BaseKey.SetValue("", ApplicationName);

            OpenMethod = Registry.ClassesRoot.CreateSubKey(ApplicationName);
            OpenMethod.SetValue("", FileDescription);
            if(Icon==null)
                OpenMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + AppPath + "\",0");
            else
                OpenMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + Icon + "\"");
            Shell = OpenMethod.CreateSubKey("Shell");
            Shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + AppPath + "\"" + " \"%1\"");
            Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + AppPath + "\"" + " \"%1\"");
            BaseKey.Close();
            OpenMethod.Close();
            Shell.Close();


/*
            Extension = "." + Extension.Trim('.').ToLowerInvariant();
            RegistryKey key = Registry.ClassesRoot.CreateSubKey(Extension);
            key.SetValue("", ApplicationName);
            key.Close();

            key = Registry.ClassesRoot.CreateSubKey(Extension + "\\Shell\\open\\command");
            //key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%L\"");
            key.SetValue("", "\"" + AppPath + "\" \"%L\"");
            key.Close();

            key = Registry.ClassesRoot.CreateSubKey(Extension + "\\Shell\\edit\\command");
            //key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%L\"");
            key.SetValue("", "\"" + AppPath + "\" \"%L\"");
            key.Close();


            key = Registry.ClassesRoot.CreateSubKey(Extension + "\\DefaultIcon");
            //key.SetValue("", Application.StartupPath + "\\icon.ico");
            key.SetValue("", IconPath);

            key.Close();*/


            // Delete the key instead of trying to change it
           /* CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ucs", true);
            CurrentUser.DeleteSubKey("UserChoice", false);
            CurrentUser.Close();
            */
            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
        public static string FileExtentionInfo(AssocStr assocStr, string Extension)
        {
            Extension = "." + Extension.Trim('.').ToLowerInvariant();
            uint pcchOut = 0;
            AssocQueryString(AssocF.Verify, assocStr, Extension, null, null, ref pcchOut);
            StringBuilder pszOut = new StringBuilder((int)pcchOut);
            AssocQueryString(AssocF.Verify, assocStr, Extension, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }

        [Flags]
        private enum AssocF
        {
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }
        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic
        }



        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, [In][Out] ref uint pcchOut);
        
        #endregion



        public delegate void CopyProgressChangedDelegate(double persentage,ref bool cancelFlag);
        public delegate void CopyCompleteDelegate(bool copiato);

    }
}
