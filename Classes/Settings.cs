using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Classes
{
    /// <summary>
    /// Modello da ereditare che permette velocemente di salvare su file / leggere da file un oggetto
    /// Creare un costruttore di default ma usare SOLO la GetInstance()
    /// </summary>
    [Serializable]
    public abstract class Settings<T> where T : class, new()
    {
        [NonSerialized]
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }




        static public String SettingPath { get; set; } = null;
        protected Settings()
        {
        }





        public void Load()
        {
            if (SettingPath == null)
                throw new Exception("Path di salvataggi non impostato");


            ResourceReader reader = new ResourceReader(SettingPath);
            _instance =(T)reader.Get("Setting");
        }
        public void Save()
        {
            if (SettingPath == null)
                throw new Exception("Path di salvataggi non impostato");


            if (!ServicesManager.IsSet<SystemService>())
            {
                ServicesManager.RegistService(new SystemService());
            }


            FilePlus file = new FilePlus(SettingPath);
            if (file.Exist())
                file.Delete();

            IResourceWriter writer = new ResourceWriter(SettingPath);
            writer.AddResource("Setting", _instance);
            writer.Close();
        }
    }
}
