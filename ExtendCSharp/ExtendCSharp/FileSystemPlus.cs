using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{

    public class FileSystemPlus: FileSystemPlus<ObjectPlus>
    {
        public FileSystemPlus(String RootPath, FileSystemPlusLoadOption option = null) : base(RootPath, option)
        {
            
        }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class FileSystemPlus<T>
        where T : ICloneablePlus,new()
    {
        #region Variabili
        [JsonProperty]
        protected String _RootRealPath = "";
        [JsonProperty]
        public String RootPath { get { return _RootRealPath; } }

        [JsonProperty]
        protected FileSystemNodePlus<T> _Root;
        [JsonProperty]
        public FileSystemNodePlus<T> Root {
            get { return _Root; }
            set { _Root = value; }
        }

        #endregion

        protected FileSystemPlus()
        {
        }
        public FileSystemPlus(String RootPath, FileSystemPlusLoadOption option = null)
        {
            if (Directory.Exists(RootPath))
            {
                RootPath = RootPath.TrimEnd('\\', '/');
                if (FileSystemPlusUtil.IsRootpath(RootPath))
                    RootPath += "\\";
                _RootRealPath = RootPath;
                _Root = new FileSystemNodePlus<T>(RootPath, null, option);
            }
            else
                throw new DirectoryNotFoundException("la cartella specificata deve essere una directory valida\r\n" + RootPath);

        }

        public void Merge(FileSystemPlus<T> OtherFileSystem)
        {
             if (_Root == null)
                _Root = new FileSystemNodePlus<T>();
            _Root.Merge(OtherFileSystem._Root);
        }
        public void Add(FileSystemPlus<T> OtherFileSystem)
        {
            if (_Root == null)
                _Root = new FileSystemNodePlus<T>();
            
            _Root.Add(OtherFileSystem._Root);
        }
        public FileSystemPlus<T> Clone()
        {
            FileSystemPlus<T> n = new FileSystemPlus<T>();
            n._RootRealPath = _RootRealPath;
            n._Root = _Root.Clone();
            n.Root.SetParentOnAllChild(FileSystemNodePlusLevelType.FirstLevel);
            return n;
        }


        public String GetFullPath(FileSystemNodePlus<T> Nodo)
        {
            return Path.Combine(_RootRealPath, Nodo.GetFullPath());
        }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class FileSystemNodePlus<T>
        where T : ICloneablePlus,new()
    {
        #region Costruttori

        public FileSystemNodePlus()
        {

        }
        public FileSystemNodePlus(FileSystemNodePlus<T> Parent = null)
        {
            _Parent = Parent;
        }
        public FileSystemNodePlus(String LoadPath,FileSystemNodePlus<T> Parent = null,FileSystemPlusLoadOption option=null)
        {
            _Parent = Parent;
            LoadFromRealPath(LoadPath, option);
        }

        #endregion

        #region Variabili

        [JsonProperty]
        FileSystemNodePlusType _Type = FileSystemNodePlusType.Directory;
        [JsonIgnore]
        public FileSystemNodePlusType Type { get { return _Type; } }

        [JsonProperty]
        String _Name="";
        [JsonIgnore]
        public String Name { get { return _Name; } }

        [JsonIgnore]
        FileSystemNodePlus<T> _Parent = null;
        [JsonIgnore]
        public FileSystemNodePlus<T> Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }
        [JsonIgnore]
        public FileSystemNodePlus<T> FirstParent
        {
            get
            {
                if (_Parent == null)
                    return this;
                else
                    return _Parent.FirstParent;
            }
   
        }


        [JsonProperty]
        Dictionary<String, FileSystemNodePlus<T>> _FileSystem = new Dictionary<string, FileSystemNodePlus<T>>();

        [JsonProperty]
        public T AddittionalData = new T();

        public int ChildCount { get { return _FileSystem.Count; } }

        #endregion
     
        public void LoadFromRealPath(String Path,FileSystemPlusLoadOption option=null)
        {
            Path = System.IO.Path.GetFullPath(Path);
            
            if (File.Exists(Path))
            {
                if (System.IO.Path.GetExtension(Path) == ".exe")
                {

                }
                if (option == null || (option != null && (!option.RestrictExtensionEnable || option.RestrictExtension.Contains(System.IO.Path.GetExtension(Path).TrimStart('.').ToLower()))))
                { 
                    _Type = FileSystemNodePlusType.File;
                    _Name = Path.SplitAndGetLast('\\', '/');
                }
            }
            else if(Directory.Exists(Path))
            {
                _Type = FileSystemNodePlusType.Directory;
                _Name = Path.SplitAndGetLast('\\', '/');
                Path += "\\";

                try
                {
                    string[] subdir = Directory.GetDirectories(Path);
                    foreach (string subdirectory in subdir)
                    {
                        String t = subdirectory.SplitAndGetLast('\\', '/');
                        _FileSystem[t] = new FileSystemNodePlus<T>(subdirectory, this, option);
                    }
                }
                catch(Exception ex)
                {
                    if (option == null || !option.IgnoreException)
                        throw ex;
                }

                try
                {
                    string[] subfs = Directory.GetFiles(Path);
                    foreach (string subfile in subfs)
                    {
                        if (option == null || (option != null && (!option.RestrictExtensionEnable || option.RestrictExtension.Contains(System.IO.Path.GetExtension(subfile).TrimStart('.').ToLower()))))
                        {
                            String t = subfile.SplitAndGetLast('\\', '/');
                            _FileSystem[t] = new FileSystemNodePlus<T>(subfile, this, option);
                        } 
                    }
                }
                catch (Exception ex)
                {
                    if (option == null || !option.IgnoreException)
                        throw ex;
                }
            }
            else
            {
                if(option==null || !option.IgnoreException)
                    throw new FileNotFoundException("Il file o cartella non è stato trovato", Path);
            }

        }


        #region Getter

        public delegate bool FuncOggettoDaSelezionare(FileSystemNodePlus<T> Nodo);
        public FileSystemNodePlus<T>[] GetAllNode(FuncOggettoDaSelezionare fn )
        {
            if (fn == null)
                return null;
            return _FileSystem.Where(pair => fn(pair.Value)).Select(pair => pair.Value).ToArray();
        }
        public FileSystemNodePlus<T>[] GetAllNode(FileSystemNodePlusType? Type=null )
        {
            if(Type==null)
                return _FileSystem.Select(pair => pair.Value).ToArray(); 
            return _FileSystem.Where(pair => pair.Value.Type == Type.Value).Select(pair => pair.Value).ToArray();
        }

        public String[] GetAllNameNode(FileSystemNodePlusType? Type = null)
        {
            if (Type == null)
                return _FileSystem.Select(pair => pair.Key).ToArray();
            return _FileSystem.Where(pair => pair.Value.Type == Type.Value).Select(pair => pair.Key).ToArray();
        }
        public String GetFullPath()
        {
            String s = "";
            if(_Parent!=null)
                s= Parent.GetFullPath()+"\\";
            s+= _Name;
            return s;
        }

        public FileSystemNodePlus<T> this[String _Path]
        {
            get {
                _Path = _Path.Trim('\\', '/');
                String[] s = _Path.Split('\\', '/');
                if(s.Length==1)
                {
                    return _FileSystem.ContainsKey(s[0]) ? _FileSystem[s[0]] : null;
                }
                else
                {
                    _Path = _Path.RemoveLeft(s[0]);
                    _Path = _Path.TrimStart('\\', '/');
                    return _FileSystem.ContainsKey(s[0]) ? _FileSystem[s[0]][_Path] : null;
                }
            }
            set {
                _Path = _Path.TrimEnd('\\', '/');
                String[] s = _Path.Split('\\', '/');
                if (s.Length == 1)
                {
                    _FileSystem[s[0]] = value;
                }
                else
                {
                    _Path = _Path.RemoveLeft(s[0]);
                    _Path = _Path.TrimStart('\\', '/');
                    _FileSystem[s[0]][_Path] = value;
                }
            }
        }

        #endregion

        #region Override

        public override string ToString()
        {
            return _Name;
        }

        #endregion


        public void Merge(FileSystemNodePlus<T> OtherNode)
        {
            if(Type==FileSystemNodePlusType.File)
            {
                throw new Exception("Errore!\r\nLa destinazione della merge non può essere un file");
            }
            foreach (KeyValuePair<String,FileSystemNodePlus<T>> kv in OtherNode._FileSystem)
            {
                if(_FileSystem.ContainsKey(kv.Key))
                {
                    if(_FileSystem[kv.Key].Type==FileSystemNodePlusType.Directory)
                    {
                        _FileSystem[kv.Key].Merge(OtherNode[kv.Key]);
                    }
                }
                else
                {
                    _FileSystem[kv.Key] = kv.Value;
                }
            }
        }
        public void Add(FileSystemNodePlus<T> OtherNode)
        {
            if (Type == FileSystemNodePlusType.File)
            {
                throw new Exception("Errore!\r\nLa destinazione dell'add non può essere un file");
            }

            if (_FileSystem.ContainsKey(OtherNode._Name))
            {
                if (_FileSystem[OtherNode._Name].Type == FileSystemNodePlusType.Directory)
                {
                    _FileSystem[OtherNode._Name].Merge(OtherNode);
                }
            }
            else
            {
                _FileSystem[OtherNode._Name] = OtherNode;
            }
        }
        public void Remove(FuncOggettoDaSelezionare fn, FileSystemNodePlusLevelType Type, FileSystemNodePlusControlType CType)
        {
            if (CType == FileSystemNodePlusControlType.Pre || CType == FileSystemNodePlusControlType.PrePost)
            {
                List<string> toRemove = _FileSystem.Where(pair => fn(pair.Value)).Select(pair => pair.Key).ToList();
                foreach (String key in toRemove)
                    _FileSystem.Remove(key);
            }

            if(Type==FileSystemNodePlusLevelType.AllNode)
                foreach (KeyValuePair<String, FileSystemNodePlus<T>> kv in _FileSystem)
                    kv.Value.Remove(fn, Type, CType);

            if (CType == FileSystemNodePlusControlType.Post || CType == FileSystemNodePlusControlType.PrePost)
            {
                List<string> toRemove = _FileSystem.Where(pair => fn(pair.Value)).Select(pair => pair.Key).ToList();
                foreach (String key in toRemove)
                    _FileSystem.Remove(key);
            }

        }
        public FileSystemNodePlus<T> Clone()
        {
            FileSystemNodePlus<T> n = new FileSystemNodePlus<T>();
            n._Type = _Type;
            n._Name = _Name;
            n.AddittionalData = AddittionalData.Clone()._Cast<T>();
            n._FileSystem = new Dictionary<string, FileSystemNodePlus<T>>();
            foreach (KeyValuePair<String, FileSystemNodePlus<T>> kv in _FileSystem)
                n._FileSystem.Add(kv.Key, kv.Value.Clone());
            n.SetParentOnAllChild(FileSystemNodePlusLevelType.FirstLevel);
            return n;
        }

        public void SetParentOnAllChild(FileSystemNodePlusLevelType type)
        {
            foreach (KeyValuePair<String, FileSystemNodePlus<T>> kv in _FileSystem)
            {
                kv.Value._Parent = this;
                if (type == FileSystemNodePlusLevelType.AllNode)
                    kv.Value.SetParentOnAllChild(type);
            }
        }
        
    }


    public class FileSystemPlusLoadOption
    {
        public bool IgnoreException = false;

        public bool RestrictExtensionEnable = false;
        public List<String> RestrictExtension = null;
        public FileSystemPlusLoadOption()
        {
            RestrictExtension = new List<string>();
        }

    }



    public class FileSystemPlusUtil
    {
        public static bool IsRootpath(String Path)
        {
            Path = Path.Trim().TrimEnd('\\', '/');
            return Path.Length == 2 && Path[1] == ':';
        }
    }

    public enum FileSystemNodePlusType
    {
        File,
        Directory
    }

    public enum FileSystemNodePlusLevelType
    {
        AllNode,
        FirstLevel
    }
    public enum FileSystemNodePlusControlType
    {
        Pre,
        Post,
        PrePost
    }

}
