using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendCSharp;
using ExtendCSharp.Services;

namespace ExtendCSharp.ExtendedClass
{
    [Serializable]
    public class FilePlus
    {
        /// <summary>
        /// Permette di creare un nuovo file passando il nome e i byte che lo compongono
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public static FilePlus Create(string name,byte[] data)
        {
            FilePlus fp = new FilePlus();
            fp.data = data;
            fp.Name = name;

            SystemService ss = ServicesManager.Get<SystemService>();
            string FileFullPath = ss.GetFullPath(name);
            fp.Folder = ss.GetParent(FileFullPath);


            return fp;
        }


        /// <summary>
        /// Nome del file
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Estensione del file
        /// </summary>
        public String Extension { get; set; }

        /// <summary>
        /// Nome + Estensione
        /// </summary>
        public String Name_Extension
        {
            get
            {
                return Name + Extension;
            }
        }


        /// <summary>
        /// Cartella contenente il file
        /// </summary>
        public String Folder { get; set; }

        /// <summary>
        /// Path Completo del file ( Folder + Nome + Estensione )
        /// </summary>
        public String Path
        {
            get
            {
                return ServicesManager.Get<SystemService>().CombinePaths(Folder, Name_Extension);
            }
        }

        public Version Version { get; set; } =null;


        public long Size { get; set; }


        public byte[] data { get; set; } = null;


        public FilePlus(String FileFullPath)
        {
            SystemService ss = ServicesManager.Get<SystemService>();
            FileFullPath = ss.GetFullPath(FileFullPath);


            Name = ss.GetFileNameWithoutExtension(FileFullPath);
            Extension = ss.GetExtension(FileFullPath);
            Folder = ss.GetParent(FileFullPath);
            String v = ss.GetFileVersion(FileFullPath);
            if (v != null)
                Version = new Version(v);

            Size = ss.GetFileSize(FileFullPath);

        }
        private FilePlus()
        {

        }


        /// <summary>
        /// Carica il file in memoria RAM
        /// </summary>
        public void Load()
        {
            data = ServicesManager.Get<SystemService>().ReadAllBytes(Path);
        }


        /// <summary>
        /// Elimina il file dalla memoria RAM
        /// </summary>
        public void Unload()
        {
            data = null;
            GC.Collect();
        }

        /// <summary>
        /// Salva il file, dalla memoria ram, nella Folder specificata
        /// </summary>
        public void Save()
        {
            if(data!=null)
                ServicesManager.Get<SystemService>().WriteAllBytes(Path,data);
        }




    }
}
