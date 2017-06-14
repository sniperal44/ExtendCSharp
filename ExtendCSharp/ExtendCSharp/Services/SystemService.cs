using ExtendCSharp.ExtendedClass;
using Microsoft.Win32;
using Services.ExtendCSharp;
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



        public static String GetFileName(String s)
        {
           return Path.GetFileName(s);
        }
        public static String GetFileNameWithoutExtension(String s)
        {
            return Path.GetFileNameWithoutExtension(s);
        }
        public static String GetExtension(String s)
        {
            return Path.GetExtension(s);
        }

        public static String GetParent(String s)
        {
            return Directory.GetParent(s).FullName;
        }

        public static String[] GetDirectories(String path)
        {
            return Directory.GetDirectories(path);
        }

        /// <summary>
        /// Ritorna un array di stringhe contenente i nomi dei file nel path passato
        /// </summary>
        /// <param name="Path">Cartella da cui leggere i file</param>
        /// <param name="ExcludedExtension">Lista di estensioni da escludere dal GET!</param>
        /// <returns></returns>
        public static String[] GetFiles(String Path,params String[] ExcludedExtension)
        {
            String[] t=Directory.GetFiles(Path);
            ListPlus<String> lp = new ListPlus<string>();
            foreach(String s in t)
            {
                bool DaEscludere = false;
                foreach(string ext in ExcludedExtension)
                {
                    if(s.EndsWith(ext.OneCharStart('.')))
                    {
                        DaEscludere = true;
                        break;
                    }
                }
                if (!DaEscludere)
                    lp.Add(s);                
            }
            return lp.ToArray();
        }


        /// <summary>
        /// Ritorna la directory dove è posizionato il file  ( se viene passata una cartella, ritorna la cartella stessa ) 
        /// </summary>
        /// <param name="PathEl">Elemento da analizzare ( file o cartella ) </param>
        /// <returns></returns>
        public static String GetDirectoryName(String PathEl)
        {
            if( FileExist(PathEl))
            {
                return GetParent(PathEl);
            }
            else if (DirectoryExist(PathEl))
            {
                return PathEl;
            }
            else
            {
                if(GetExtension(PathEl)=="")//cartella
                    return PathEl;
                else
                    return GetParent(PathEl);
            }
        }



        public static String ChangeExtension(String Path,String Ext)
        {
            int sindex = Path.LastIndexOf('.');
            if (sindex == -1)
                return Path;
            return Path.Remove(sindex) + '.' + Ext.Trim(' ', '.').ToLower();
        }

        public static String CombinePaths(params string[] paths)
        { 
            String p= Path.Combine(paths);
            if (p.EndsWith(":"))
                p += "\\";
            return p;
        }
        /// <summary>
        /// Restituisce un path relativo partendo dalla AbsolutePath sulla base di BasePath
        /// </summary>
        /// <param name="AbsolutePath"></param>
        /// <param name="BasePath"></param>
        /// <returns></returns>
        public static String RelativePathFromBase(String AbsolutePath, String BasePath)
        {
            String[] abs = AbsolutePath.Split('\\', '/');
            String[] bas = BasePath.Split('\\', '/');

            int max = Math.Min(abs.Length, bas.Length);

            int i = 0;
            while(i< max)
            {
                if(abs[i].ToUpperInvariant()!=bas[i].ToUpperInvariant())
                {
                    break;
                }
                i++;
            }
            String[] Rel = abs.SubArray(i);
            return CombinePaths(Rel);
            
        }



        public static String GetCommonPath(string path1, string path2)
        {
            String p1 = GetFullPath(path1).ToUpperInvariant();
            String p2 = GetFullPath(path2).ToUpperInvariant();

            string[] a1 = p1.Split('\\', '/');
            string[] a2 = p2.Split('\\', '/');

            int n = a1.Length < a2.Length ? a1.Length : a2.Length;
            int i = 0;
            String FinalPath = "";
            for(;i<n;i++)
            {
                if(a1[i]==a2[i])
                    FinalPath=CombinePaths(FinalPath, a1[i]);
                else
                    break;
            }
            return FinalPath;
        }
        public static String GetCommonPath(params string[] paths)
        {
            if (paths.Length == 0)
                return null;
            else if (paths.Length == 1)
                return paths[0];
            else
            { 
                String Common= GetCommonPath(paths[0], paths[1]);
                for (int i=2;i<paths.Length;i++)
                {
                    Common = GetCommonPath(Common, paths[i]);
                }
                return Common;
            }

        }




        public static bool CopySecure(String Source,String Dest, bool Override=true,CopyProgressChangedDelegate OnProgressChanged =null, CopyCompleteDelegate OnComplete=null)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool cancelFlag = false;

            try
            {
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
                                OnComplete(true,null);
                            return true;
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
                if (OnComplete != null)
                    OnComplete(true,null);
                return true;
            }
            catch (Exception ex)
            {
                if (OnComplete != null)
                    OnComplete(false,ex);
                return false;
            }
        }


        public static void Rename(String Source, String Dest, bool Override = true)
        {
            if (!FileExist(Source))
                return;
            if (!Override && File.Exists(Dest))
                return;
            if (!DirectoryExist(GetParent(Dest)))
                CreateFolderSecure(GetParent(Dest));
            File.Move(Source, Dest);
        }


        /// <summary>
        /// Permette di cancellare in maniera sicura un file o cartella ritornando l'esito dell'operazione
        /// </summary>
        /// <param name="Path"></param>
        public static bool DeleteSecure(String Path)
        { 
            if (FileExist(Path))
            {
                try
                {
                    File.Delete(Path);
                }
                catch (Exception e) { }

                return !FileExist(Path);
            }
            else if(DirectoryExist(Path))
            {
                try
                {


                    string[] files = GetFiles(Path);
                    string[] dirs = GetDirectories(Path);

                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        DeleteSecure(file);
                    }

                    foreach (string dir in dirs)
                    {
                        DeleteSecure(dir);
                    }

                    Directory.Delete(Path, false);
                }
                catch (Exception e){}

                return !DirectoryExist(Path);
            }

            return true;
        }


        public static bool FileExist(String Path)
        {
            return File.Exists(Path);
        }
        public static bool DirectoryExist(String Path)
        {
            return Directory.Exists(Path);
        }
        /// <summary>
        /// Controlla se il path esiste ( file o cartella ) 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool Exist(String Path)
        {
            return FileExist(Path) || DirectoryExist(Path);
        }




        public static bool DirectoryIsEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public static long FileSize(String Path)
        {
            return new FileInfo(Path).Length;
        }


        public static DriveInfo GetDriveInfo(String path)
        {
            return new DriveInfo(Path.GetPathRoot(path));
        }





        public static String GetMD5(String Path, MD5BlockTransformEventHandler OnMD5BlockTransform, MD5ComputeHashFinishEventHandler OnMD5ComputeHashFinish,bool Async=true)
        {
            try
            {
                byte[] HashReturn=null;
                MD5Plus md5 = new MD5Plus();
                var stream = File.OpenRead(Path);
                md5.OnMD5BlockTransformEventHandler += OnMD5BlockTransform;
                md5.OnMD5ComputeHashFinishEventHandler += OnMD5ComputeHashFinish;
                md5.OnMD5ComputeHashFinishEventHandler += (byte[] Hash)=>
                {
                    stream.Close();
                    stream.Dispose();
                    
                };

                md5.ComputeHashMultiBlockAsync(stream).Join();
                if (HashReturn != null)
                    return HashReturn.ToHexString(); 

            }
            catch (Exception ex)
            {
                OnMD5ComputeHashFinish?.Invoke(null);
            }
            return "";
        }


        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
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



        public delegate void CopyProgressChangedDelegate(double percent,ref bool cancelFlag);
        public delegate void CopyCompleteDelegate(bool copiato,Exception ex);

    }

 
}
