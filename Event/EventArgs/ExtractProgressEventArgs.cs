using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ZipService")]


namespace ExtendCSharp.Event.EventArgs
{
   
    public class ExtractProgressEventArgs
    {

        Ionic.Zip.ExtractProgressEventArgs inter;

        #region Constructor

        public ExtractProgressEventArgs(Ionic.Zip.ExtractProgressEventArgs inter)
        {
            this.inter = inter;
        }

        #endregion

        #region Fields

        //AGGIUNTI
        private float _PercentByteTotal;
        private Int32 _EntriesExtracted;
        private Int32 _EntriesTotal;
        private Int64 _TotalByteTransferred;
        private Int64 _TotalByteToTransfer;

        #endregion

        #region Properties

        public Int32 EntriesExtracted       //Buggate -> riscritta
        {
            get
            {
                return _EntriesExtracted;
            }
            internal set
            {
                _EntriesExtracted = value;
            }
        }
        public String ExtractLocation
        {
            get
            {
                return inter.ExtractLocation;
            }
        }
        public Int32 EntriesTotal           //Buggate -> riscritta
        {
            get
            {
                return _EntriesTotal;
            }
            internal set
            {
                _EntriesTotal = value;
            }
        }
        public Ionic.Zip.ZipEntry CurrentEntry
        {
            get
            {
                return inter.CurrentEntry;
            }
            set
            {
                inter.CurrentEntry = value;
            }
        }
        public bool Cancel
        {
            get
            {
                return inter.Cancel;
            }
            set
            {
                inter.Cancel = value;
            }
        }
        public Ionic.Zip.ZipProgressEventType EventType
        {
            get
            {
                return inter.EventType;
            }
            set
            {
                inter.EventType = value;
            }
        }
        public String ArchiveName
        {
            get
            {
                return inter.ArchiveName;
            }
            set
            {
                inter.ArchiveName = value;
            }
        }
        /// <summary>
        /// Byte trasferiti dell'Entry corrente
        /// </summary>
        public Int64 EntryBytesTransferred
        {
            get
            {
                return inter.BytesTransferred;
            }
            internal set
            {
                inter.BytesTransferred = value;
            }
        }      //Nome non adeguato -> cambiato
        /// <summary>
        /// Byte Totali da trasferire dell'entry corrente
        /// </summary>
        public Int64 EntryBytesToTransfer
        {
            get
            {
                return inter.TotalBytesToTransfer;
            }
            internal set
            {
                inter.TotalBytesToTransfer = value;
            }
        }       //Nome non adeguato -> cambiato

        /// <summary>
        /// Byte Totali trasferiti di tutta l'estrazione corrente
        /// </summary>
        public Int64 TotalByteTransferred
        {
            get
            {
                return _TotalByteTransferred;
            }
            internal set
            {
                _TotalByteTransferred = value;
            }
        }    //Aggiunta
        /// <summary>
        /// Byte Totali da trasferire di tutta l'estrazione
        /// </summary>
        public Int64 TotalByteToTransfer
        {
            get
            {
                return _TotalByteToTransfer;
            }
            internal set
            {
                _TotalByteToTransfer = value;
            }
        }     //Aggiunta


        //AGGIUNTI
        public float EntryPercentByte
        {
            get
            {
                return 100 * (float)EntryBytesTransferred / EntryBytesToTransfer;
            }
        }
        public float TotalPercentByte
        {
            get
            {
                return 100 * (float)TotalByteTransferred / TotalByteToTransfer;
            }
        }
        public float FileNumberPercentFile
        {
            get
            {
                return 100 * (float)EntriesExtracted / EntriesTotal;
            }
        }
       

        #endregion

        #region Methods

        public String ToString()
        {
            return inter.ToString();
        }

        public Boolean Equals(object obj)
        {
            return inter.Equals(obj);
        }

        public Int32 GetHashCode()
        {
            return inter.GetHashCode();
        }

        public Type GetType()
        {
            return inter.GetType();
        }

        #endregion

        #region Events

        private void EventAssotiation()
        {
        }
        #endregion


    }
}
