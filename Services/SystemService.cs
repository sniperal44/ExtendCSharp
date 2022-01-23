using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtendCSharp.Services
{
    public class SystemService : IService
    {
        public bool  CreateFolderSecure(String Path)
        {
            if(Directory.Exists(Path))
                return true;
            return Directory.CreateDirectory(Path).Exists; 
        }


        /// <summary>
        ///  Restituisce il nome del file e l'estensione della stringa di percorso specificata.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String GetFileName(String s)
        {
           return Path.GetFileName(s);
        }
        /// <summary>
        ///  Restituisce il nome del file della stringa di percorso specificata.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String GetFileNameWithoutExtension(String s)
        {
            return Path.GetFileNameWithoutExtension(s);
        }
        /// <summary>
        ///  Restituisce l'estensione della stringa di percorso specificata.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String GetExtension(String s)
        {
            return Path.GetExtension(s);
        }
        /// <summary>
        ///  Restituisce la versione del file della stringa di percorso specificata.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String GetFileVersion(String path)
        {
            return FileVersionInfo.GetVersionInfo(path).FileVersion;
        }
        public long GetFileSize(String path)
        {
            return new System.IO.FileInfo(path).Length;
        }


        /// <summary>
        /// Recupera la directory padre del percorso specificato, inclusi il percorso assoluto e relativo.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String GetParent(String s)
        {
            return Directory.GetParent(s).FullName;
        }
        /// <summary>
        /// Restituisce il percorso assoluto della stringa di percorso specificata.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Ritorna un array di stringhe contenente i nomi delle cartelle nel path passato
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String[] GetDirectories(String path)
        {
            return Directory.GetDirectories(path);
        }
        /// <summary>
        /// Ritorna un array di stringhe contenente i nomi dei file nel path passato
        /// </summary>
        /// <param name="Path">Cartella da cui leggere i file</param>
        /// <param name="ExcludedExtension">Lista di estensioni da escludere dal GET!</param>
        /// <returns></returns>
        public String[] GetFiles(String Path,params String[] ExcludedExtension)
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
        /// Ritorna un array di path ( stringhe ) a tutti i file contenuti in un determinato path
        /// ATTENZIONE! potrebbe essere molto lento!
        /// </summary>
        /// <param name="Path">Cartella root di ricerca</param>
        /// <returns></returns>
        public string[] GetFilesInTree(String Path,String Regex="")
        {
            ListPlus<String> lp = new ListPlus<string>();
            string[] files;
            if (Regex != "")
                files = FindRegex(Path, Regex);
            else
                files = Directory.GetFiles(Path);
            lp.AddRange(files);
            foreach (string d in Directory.GetDirectories(Path))
            {
                lp.AddRange(GetFilesInTree(d, Regex));
            }
            return lp.ToArray();
        }


        /// <summary>
        /// Ritorna la directory dove è posizionato il file  ( se viene passata una cartella, ritorna la cartella stessa ) 
        /// </summary>
        /// <param name="PathEl">Elemento da analizzare ( file o cartella ) </param>
        /// <returns></returns>
        public String GetDirectoryName(String PathEl)
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

        public String GetExecutingDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public String ChangeExtension(String Path,String Ext)
        {
            int sindex = Path.LastIndexOf('.');
            if (sindex == -1)
                return Path;
            return Path.Remove(sindex) + '.' + Ext.Trim(' ', '.').ToLower();
        }

        public String CombinePaths(params string[] paths)
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
        public String RelativePathFromBase(String AbsolutePath, String BasePath)
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

        /// <summary>
        /// Ritorna il path della cartella temporanea dell'utente
        /// /// </summary>
        /// <returns></returns>
        public String GetTempPath()
        {
            return Path.GetTempPath();
        }


        public String GetCommonPath(string path1, string path2)
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
        public String GetCommonPath(params string[] paths)
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




        public bool CopySecure(String Source,String Dest, bool Override=true,CopyProgressChangedDelegate OnProgressChanged =null, CopyCompleteDelegate OnComplete=null)
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
                            OnComplete?.Invoke(true, null);
                            return true;
                        }

                    }

                    CreateFolderSecure(GetParent(Dest));
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
                OnComplete?.Invoke(true, null);
                return true;
            }
            catch (Exception ex)
            {
                OnComplete?.Invoke(false, ex);
                return false;
            }
        }

        public void Write(String Path,String Contents, bool Append)
        {
            if (Append)
                File.AppendAllText(Path, Contents);
            else
                File.WriteAllText(Path, Contents);
        }
        public void Write(String Path, String Contents, Encoding encoding, bool Append)
        {
            if (Append)
                File.AppendAllText(Path, Contents, encoding);
            else
                File.WriteAllText(Path, Contents, encoding);
        }
        public void WriteAllBytes(String Path, byte[] data)
        {
            CreateFolderSecure(GetParent(Path));
            File.WriteAllBytes(Path, data);
        }



        public String Read(String Path, Encoding encoding)
        {
            return File.ReadAllText(Path, encoding);
        }
        public String Read(String Path)
        {
            return File.ReadAllText(Path);
        }
        public byte[] ReadAllBytes(String Path)
        {
            return File.ReadAllBytes(Path);
        }


        public void Rename(String Source, String Dest, bool Override = true)
        {
            if (FileExist(Source))
            {
                if (!Override && FileExist(Dest))
                    return;
                if (!DirectoryExist(GetParent(Dest)))
                    CreateFolderSecure(GetParent(Dest));
                File.Move(Source, Dest);
            }
            else if ( DirectoryExist(Source))
            {
                if (!DirectoryExist(GetParent(Dest)))
                    CreateFolderSecure(GetParent(Dest));
                try
                {
                    Directory.Move(Source, Dest);
                }
                catch(Exception)
                { }
            }
        }


        /// <summary>
        /// Permette di cancellare in maniera sicura un file o cartella ritornando l'esito dell'operazione
        /// </summary>
        /// <param name="Path"></param>
        public bool DeleteSecure(String Path)
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
                catch (Exception ){}

                return !DirectoryExist(Path);
            }

            return true;
        }


        public bool FileExist(String Path)
        {
            return File.Exists(Path);
        }
        public bool DirectoryExist(String Path)
        {
            return Directory.Exists(Path);
        }
        /// <summary>
        /// Controlla se il path esiste ( file o cartella ) 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public bool Exist(String Path)
        {
            return FileExist(Path) || DirectoryExist(Path);
        }

        
        public String[] Find(String Path,String pattern)
        {
            return Directory.GetFiles(Path, pattern);
        }
        public String[] FindRegex(String Path, String pattern)
        {
            String[] Files= Directory.GetFiles(Path);
            List<String> ReturnStrings = new List<string>();

            Regex rgx = new Regex(pattern, RegexOptions.Singleline);

            foreach (String file in Files )
            {
                if (rgx.IsMatch(GetFileName(file)))
                    ReturnStrings.Add(file);
            }
            
            return ReturnStrings.ToArray();
        }



        public bool DirectoryIsEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public long FileSize(String Path)
        {
            return new FileInfo(Path).Length;
        }


        public DriveInfo GetDriveInfo(String path)
        {
            return new DriveInfo(Path.GetPathRoot(path));
        }





        public String GetMD5(String Path, MD5BlockTransformEventHandler OnMD5BlockTransform, MD5ComputeHashFinishEventHandler OnMD5ComputeHashFinish,bool Async=true)
        {
            
            
            try
            {
                byte[] HashReturn=null;
                MD5Service md5 = new MD5Service();
                var stream = File.OpenRead(Path);
                md5.OnMD5BlockTransformEventHandler += OnMD5BlockTransform;
                md5.OnMD5ComputeHashFinishEventHandler += OnMD5ComputeHashFinish;
                md5.OnMD5ComputeHashFinishEventHandler += (byte[] Hash)=>
                {
                    stream.Close();
                    stream.Dispose();   
                };

                md5.ComputeHashMultiBlockThread(stream).Join();
                if (HashReturn != null)
                    return HashReturn.ToHexString(); 

            }
            catch (Exception )
            {
                OnMD5ComputeHashFinish?.Invoke(null);
            }
            return "";
        }


      
        public string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }


        #region Extension - File Association

        public void SetAssociationFileExtention(string Extension, string ApplicationName, string AppPath, string FileDescription,string Icon=null)
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
        public string FileExtentionInfo(AssocStr assocStr, string Extension)
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


        #region FileOperationAPIWrapper
        /// <summary>
        /// Possible flags for the SHFileOperation method.
        /// </summary>
        [Flags]
        public enum FileOperationFlags : ushort
        {
            /// <summary>
            /// Do not show a dialog during the process
            /// </summary>
            FOF_SILENT = 0x0004,
            /// <summary>
            /// Do not ask the user to confirm selection
            /// </summary>
            FOF_NOCONFIRMATION = 0x0010,
            /// <summary>
            /// Delete the file to the recycle bin.  (Required flag to send a file to the bin
            /// </summary>
            FOF_ALLOWUNDO = 0x0040,
            /// <summary>
            /// Do not show the names of the files or folders that are being recycled.
            /// </summary>
            FOF_SIMPLEPROGRESS = 0x0100,
            /// <summary>
            /// Surpress errors, if any occur during the process.
            /// </summary>
            FOF_NOERRORUI = 0x0400,
            /// <summary>
            /// Warn if files are too big to fit in the recycle bin and will need
            /// to be deleted completely.
            /// </summary>
            FOF_WANTNUKEWARNING = 0x4000,
        }

        /// <summary>
        /// File Operation Function Type for SHFileOperation
        /// </summary>
        public enum FileOperationType : uint
        {
            /// <summary>
            /// Move the objects
            /// </summary>
            FO_MOVE = 0x0001,
            /// <summary>
            /// Copy the objects
            /// </summary>
            FO_COPY = 0x0002,
            /// <summary>
            /// Delete (or recycle) the objects
            /// </summary>
            FO_DELETE = 0x0003,
            /// <summary>
            /// Rename the object(s)
            /// </summary>
            FO_RENAME = 0x0004,
        }



        /// <summary>
        /// SHFILEOPSTRUCT for SHFileOperation from COM
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCT
        {

            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public FileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public FileOperationFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        /// <summary>
        /// Send file to recycle bin
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        /// <param name="flags">FileOperationFlags to add in addition to FOF_ALLOWUNDO</param>
        public bool DeleteToRecicleBin(string path, FileOperationFlags flags)
        {
            try
            {
                var fs = new SHFILEOPSTRUCT
                {
                    wFunc = FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                    fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
                };
                SHFileOperation(ref fs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Send file to recycle bin.  Display dialog, display warning if files are too big to fit (FOF_WANTNUKEWARNING)
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        public bool DeleteToRecicleBin(string path)
        {
            return DeleteToRecicleBin(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING);
        }

        /// <summary>
        /// Send file silently to recycle bin.  Surpress dialog, surpress errors, delete if too large.
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        public bool DeleteToRecicleBinSilent(string path)
        {
            return DeleteToRecicleBin(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_SILENT);

        }

        /*private bool deleteFile(string path, FileOperationFlags flags)
        {
            try
            {
                var fs = new SHFILEOPSTRUCT
                {
                    wFunc = FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                    fFlags = flags
                };
                SHFileOperation(ref fs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

        /*public bool DeleteCompletelySilent(string path)
        {
            return deleteFile(path,
                              FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI |
                              FileOperationFlags.FOF_SILENT);
        }*/



        public Process Exec(string Command,params string[] args)
        {
            return Process.Start(Command,String.Join(" ",args));
        }

        #endregion

        #region GetInfoFromTheSistem

            

        #endregion


        public delegate void CopyProgressChangedDelegate(double percent,ref bool cancelFlag);
        public delegate void CopyCompleteDelegate(bool copiato,Exception ex);

    }

 
}
