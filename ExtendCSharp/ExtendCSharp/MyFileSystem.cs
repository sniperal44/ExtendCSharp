using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{

    public class MyFileSystem: MyFileSystem<object>
    {
        public MyFileSystem(String RootPath, MyFileSystemLoadOption option = null) : base(RootPath, option)
        {
            
        }
    }

    public class MyFileSystem<T>
        where T : new()
    {
        String _RootRealPath = "";
        public String RootPath { get { return _RootRealPath; } }

        MyFileSystemNode<T> _Root;
        public MyFileSystemNode<T> Root {
            get { return _Root; }
        }

        public MyFileSystem(String RootPath, MyFileSystemLoadOption option = null)
        {
            if (Directory.Exists(RootPath))
            {
                RootPath = RootPath.TrimEnd('\\', '/');
                if (MyFileSystemUtil.IsRootpath(RootPath))
                    RootPath += "\\";
                _RootRealPath = RootPath;
                _Root = new MyFileSystemNode<T>(RootPath, null, option);
            }
            else
                throw new DirectoryNotFoundException("la cartella specificata deve essere una directory valida\r\n" + RootPath);

        }

        public void Merge(MyFileSystem<T> OtherFileSystem)
        {
            _Root.Merge(OtherFileSystem._Root);
        }
        public void Add(MyFileSystem<T> OtherFileSystem)
        {
            _Root.Add(OtherFileSystem._Root);
        }


        public String GetFullPath(MyFileSystemNode<T> Nodo)
        {
            return Path.Combine(_RootRealPath, Nodo.GetFullPath());
        }
    }

    public class MyFileSystemNode<T>
        where T : new()
    {
        #region Costruttori

        public MyFileSystemNode(MyFileSystemNode<T> Parent = null)
        {
            _Parent = Parent;
        }
        public MyFileSystemNode(String LoadPath,MyFileSystemNode<T> Parent = null,MyFileSystemLoadOption option=null)
        {
            _Parent = Parent;
            LoadFromRealPath(LoadPath, option);
        }

        #endregion

        #region Variabili

        MyFileSystemNodeType _Type;
        public MyFileSystemNodeType Type { get { return _Type; } }

        String _Name="";
        public String Name { get { return _Name; } }

        MyFileSystemNode<T> _Parent = null;
        public MyFileSystemNode<T> Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }


        Dictionary<String, MyFileSystemNode<T>> _FileSystem = new Dictionary<string, MyFileSystemNode<T>>();


        public T AddittionalData = new T();


        #endregion
     
        public void LoadFromRealPath(String Path,MyFileSystemLoadOption option=null)
        {
            Path = System.IO.Path.GetFullPath(Path);
            
            if (File.Exists(Path))
            {
                if (System.IO.Path.GetExtension(Path) == ".exe")
                {

                }
                if (option == null || (option != null && (!option.RestrictExtensionEnable || option.RestrictExtension.Contains(System.IO.Path.GetExtension(Path).TrimStart('.')))))
                { 
                    _Type = MyFileSystemNodeType.File;
                    _Name = Path.SplitAndGetLast('\\', '/');
                }
            }
            else if(Directory.Exists(Path))
            {
                _Type = MyFileSystemNodeType.Directory;
                _Name = Path.SplitAndGetLast('\\', '/');
                Path += "\\";

                try
                {
                    string[] subdir = Directory.GetDirectories(Path);
                    foreach (string subdirectory in subdir)
                    {
                        String t = subdirectory.SplitAndGetLast('\\', '/');
                        _FileSystem[t] = new MyFileSystemNode<T>(subdirectory, this, option);
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
                        if (option == null || (option != null && (!option.RestrictExtensionEnable || option.RestrictExtension.Contains(System.IO.Path.GetExtension(subfile).TrimStart('.')))))
                        {
                            String t = subfile.SplitAndGetLast('\\', '/');
                            _FileSystem[t] = new MyFileSystemNode<T>(subfile, this, option);
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

        public MyFileSystemNode<T>[] GetAllNode(MyFileSystemNodeType? Type=null )
        {
            if(Type==null)
                return _FileSystem.Select(pair => pair.Value).ToArray(); 
            return _FileSystem.Where(pair => pair.Value.Type == Type.Value).Select(pair => pair.Value).ToArray();
        }
        public String[] GetAllNameNode(MyFileSystemNodeType? Type = null)
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

        public MyFileSystemNode<T> this[String _Path]
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


        public void Merge(MyFileSystemNode<T> OtherNode)
        {
            if(Type==MyFileSystemNodeType.File)
            {
                throw new Exception("Errore!\r\nLa destinazione della merge non può essere un file");
            }
            foreach (KeyValuePair<String,MyFileSystemNode<T>> kv in OtherNode._FileSystem)
            {
                if(_FileSystem.ContainsKey(kv.Key))
                {
                    if(_FileSystem[kv.Key].Type==MyFileSystemNodeType.Directory)
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
        public void Add(MyFileSystemNode<T> OtherNode)
        {
            if (Type == MyFileSystemNodeType.File)
            {
                throw new Exception("Errore!\r\nLa destinazione dell'add non può essere un file");
            }

            if (_FileSystem.ContainsKey(OtherNode._Name))
            {
                if (_FileSystem[OtherNode._Name].Type == MyFileSystemNodeType.Directory)
                {
                    _FileSystem[OtherNode._Name].Merge(OtherNode);
                }
            }
            else
            {
                _FileSystem[OtherNode._Name] = OtherNode;
            }
        }
    }

    public class MyFileSystemLoadOption
    {
        public bool IgnoreException = false;

        public bool RestrictExtensionEnable = false;
        public List<String> RestrictExtension = null;
        public MyFileSystemLoadOption()
        {
            RestrictExtension = new List<string>();
        }

    }



    public class MyFileSystemUtil
    {
        public static bool IsRootpath(String Path)
        {
            Path = Path.Trim().TrimEnd('\\', '/');
            return Path.Length == 2 && Path[1] == ':';
        }
    }

    public enum MyFileSystemNodeType
    {
        File,
        Directory
    }
}
