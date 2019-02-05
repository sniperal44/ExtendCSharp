using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class UpdateService: IService
    {
        String _VersionFileUrl = "";
        public String VersionFileUrl { get => _VersionFileUrl; }

        Assembly assembly;

        public UpdateService(Assembly assembly, String VersionFileUrl)
        {
            _VersionFileUrl = VersionFileUrl;
            this.assembly = assembly;
        }


        //Scarica il file VersionFileUrl e controlla se la versione dell'assembly corrente, è uguale alla versione dell'assembly nel file

        public VersionInfoUpdate CheckForUpdate()
        {
            //Scarico il file
            byte[] data = ServicesManager.Get<OnlineServices>().GetFileViaHttp(_VersionFileUrl);

            //Decompilo il json
            VersionFile v=null;
            if( data!=null)
                using (Stream s = data.ToStream())
                    v=ServicesManager.Get<JsonService>().Deserialize<VersionFile>(s);
         

            //ricavo la versione corrente
            Version currentVersion = assembly.GetName().Version;

            //ritorno la VersionInfoUpdate
            return new VersionInfoUpdate(currentVersion, v == null ? null:new Version(v.Version), v == null ? null : v.InstallerURL);
        }
    }

    public class VersionInfoUpdate
    {
        public Version CurrentVersion;
        public Version OnlineVersion;
        public String InstallerURL;

        
        public VersionInfoUpdate(Version CurrentVersion, Version OnlineVersion, String InstallerURL)
        {
            this.CurrentVersion = CurrentVersion;
            this.OnlineVersion = OnlineVersion;
            this.InstallerURL = InstallerURL;
        }
        public bool ToUpdate { get
            {
                if (CurrentVersion != null && OnlineVersion != null)
                    if (OnlineVersion > CurrentVersion)
                        return true;
                return false;
            }
        }
    }

    class VersionFile
    {
        public String Version;
        public String InstallerURL;
    }
}
