using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace ExtendCSharp.Services
{

    /// <summary>
    /// Servizio che espone degli eventi per l'ascolto delle connessioni/rimozioni di devices
    /// </summary>
    public class DevicesServices:IService
    {
        private DeviceServicesControl c;        //internamente sfrutto il DeviceServicesControl


        public event DeviceEventDelegate DeviceRemoved;
        public event DeviceEventDelegate DeviceAdded;


        public DevicesServices()
        {
            c = new DeviceServicesControl();
            c.RegisterHandler();

        
            c.DeviceAdded += (string deviceDescriptor,char driveLetter, DeviceTypes DeviceType) =>
            {
                DeviceAdded?.Invoke(deviceDescriptor, driveLetter, DeviceType);
            };
            c.DeviceRemoved += (string deviceDescriptor, char driveLetter, DeviceTypes DeviceType) =>
            {
                DeviceRemoved?.Invoke(deviceDescriptor, driveLetter, DeviceType);
            };

        }


        public void RegisterHandler()
        {
            c.RegisterHandler();
        }
        public void UnRegisterHandler()
        {
            c.UnRegisterHandler();
        }


    }


    /// <summary>
    /// Classe che estende un Control, necessaria per l'override del metodo WndProc
    /// </summary>
    internal class DeviceServicesControl:Control
    {
        //Variabili
        private DevicesNotification usb;


        //Eventi
        public event DeviceEventDelegate DeviceRemoved;
        public event DeviceEventDelegate DeviceAdded;
        

        //Costruttori
        public DeviceServicesControl()
        {
            usb = new DevicesNotification();
        }

        
        //Override
        protected override void WndProc(ref Message m)
        {
            //m.Msg     ->  Tipo di evento
            //m.WParam  ->  Tipo di evento specifico
            //m.LParam  ->  puntatore a strutture dati specifiche


            base.WndProc(ref m);
            if (m.Msg == DevicesNotificationConst.WM_DEVICECHANGE)      //Controllo se è un evento che interessa i Device
            {
                if ((int)m.WParam == DevicesNotificationConst.DBT_DEVICEREMOVECOMPLETE || (int)m.WParam == DevicesNotificationConst.DBT_DEVICEARRIVAL)  //Controllo se è un evento che gestisco
                {
                    string deviceDescriptor = "";
                    char driveLetter = '\0';
                    DeviceTypes deviceType = DeviceTypes._NotSet;
                    //Casto il puntatore LParam ad una struttura dati
                    DEV_BROADCAST_HDR hdr;
                    hdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));

                    if (hdr.dbch_devicetype == DevicesNotificationConst.DBT_DEVTYP_DEVICEINTERFACE) //Se è un device generico
                    {
                        deviceType = DeviceTypes.Interface;


                        DEV_BROADCAST_DEVICEINTERFACE deviceInterface = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
                        deviceDescriptor = new string(deviceInterface.dbcc_name);   //recupero il descrittore del device

                    }
                    if (hdr.dbch_devicetype == DevicesNotificationConst.DBT_DEVTYP_VOLUME)      //Se è anche un volume
                    {
                        deviceType = DeviceTypes.Volume;


                        DEV_BROADCAST_VOLUME volume;
                        volume = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));

                        //Traduco la maschera di bit in Lettera
                        driveLetter = DriveMaskToLetter(volume.dbcv_unitmask);
                    }



                    switch ((int)m.WParam)
                    {
                        case DevicesNotificationConst.DBT_DEVICEREMOVECOMPLETE:     //Se è un evento di rimozione
                            DeviceRemoved?.Invoke(deviceDescriptor, driveLetter, deviceType);  //Sollevo l'evento
                            break;
                        case DevicesNotificationConst.DBT_DEVICEARRIVAL:            //Se è un evento di inserimento
                            DeviceAdded?.Invoke(deviceDescriptor, driveLetter, deviceType);     //Sollevo l'evento
                            break;
                    }
                }
            }
        }


        //Metodi
        public void RegisterHandler()
        {
            usb.RegisterDeviceNotification(this.Handle);
        }
        public void UnRegisterHandler()
        {
            usb.UnregisterUsbDeviceNotification();
        }


        //Static Metodi
        /// <summary>
        /// Converte una maschera di bit nella corrispettiva lettera associata al Device
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 1 = A
            // 2 = B
            // 4 = C...
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)
            {
                // while there is any bit set in the mask
                // shift it to the righ...        
                pom = pom / 2;
                cnt++;
            }

            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';

            return letter;
        }
    }

    
    /// <summary>
    /// Classe a basso che gestisce l'ascolto dei dispositivi a basso livello.
    /// </summary>
    internal class DevicesNotification
    {
        private readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // identifica USB devices
        private IntPtr notificationHandle;      //handler del gestore della notifica

        /// <summary>
        /// Registra una componente ( form, controller, o qualsiasi cosa che abbia un Handle ) a ricevere una notifica quando un Device viene collegato o scollegato.
        /// </summary>
        /// <param name="windowHandle">Handle del componente dove ricevere la notifica (override di WndProc).</param>
        /// <param name="usbOnly">true = solo USB, false = tutti i dispositivi.</param>
        public void RegisterDeviceNotification(IntPtr windowHandle, bool usbOnly = false)
        {

            DEV_BROADCAST_DEVICEINTERFACE dbi = new DEV_BROADCAST_DEVICEINTERFACE
            {
                dbcc_devicetype = DevicesNotificationConst.DBT_DEVTYP_DEVICEINTERFACE,
                dbcc_reserved = 0,
                dbcc_classguid = GuidDevinterfaceUSBDevice,
                dbcc_name = new char[256]
            };


            dbi.dbcc_size = Marshal.SizeOf(dbi);
            IntPtr buffer = Marshal.AllocHGlobal(dbi.dbcc_size);
            Marshal.StructureToPtr(dbi, buffer, true);

            //Dopo aver allocato in memoria la struttura dati necessaria a contenere la notifica
            //registro la notifica all'windowHandle passato, indicando il buffer e il tipo di dispositivo da "ascoltare"
            notificationHandle = RegisterDeviceNotification(windowHandle, buffer, usbOnly ? 0 : DevicesNotificationConst.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
        }

        /// <summary>
        /// Rimuove la registrazione del componente per le notifiche
        /// </summary>
        public void UnregisterUsbDeviceNotification()
        {
            UnregisterDeviceNotification(notificationHandle);
        }



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);


        
    }


    #region Delegate

    public delegate void DeviceEventDelegate(string deviceDescriptor, char driveLetter, DeviceTypes DeviceType);

    #endregion

    #region Enum & Const

    internal static class DevicesNotificationConst
    {
        public const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device      
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // device is gone      


        public const int WM_DEVICECHANGE = 0x219; // device change event     
        public const int DBT_DEVTYP_VOLUME = 0x00000002;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;


        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

    }
    public enum DeviceTypes
    {
        Interface,
        Volume,
        _NotSet
    }

    #endregion

    #region Struct

    [StructLayout(LayoutKind.Sequential)]
    internal class DEV_BROADCAST_HDR
    {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DEV_BROADCAST_DEVICEINTERFACE
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;
        public Guid dbcc_classguid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] dbcc_name;
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct DEV_BROADCAST_VOLUME
    {
        public int dbcv_size;
        public int dbcv_devicetype;
        public int dbcv_reserved;
        public int dbcv_unitmask;
    }


    #endregion

    #region Exception
    public class DeviceTypeNotRecognizedExcepotion:Exception
    {
        public int DeviceType;
        public DeviceTypeNotRecognizedExcepotion(int DeviceType)
        {
            this.DeviceType = DeviceType;
        }
    }
    #endregion
}
