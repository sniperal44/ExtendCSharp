using ExtendCSharp.ExtendedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class SaveFileDialogPlus
    {

        SaveFileDialog inter;

        public SaveFileDialogPlus()
        {
            inter = new SaveFileDialog();
            EventAssotiation();
        }


        /// <summary>
        /// Permette di mostrare il Dialog in modalità bloccante senza aver problemi di STA Thread
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialogNewThread()
        {
            DialogResult r=DialogResult.Cancel;
            ThreadPlus tp = new ThreadPlus(() =>
            {
                r = inter.ShowDialog();
            });

            tp.SetApartmentState(System.Threading.ApartmentState.STA);
            tp.Start();
            tp.Join();

            return r;
        }




        public Stream OpenFile()
        {
            return inter.OpenFile();
        }

        public void Reset()
        {
            inter.Reset();
        }

        public String ToString()
        {
            return inter.ToString();
        }

        public DialogResult ShowDialog()
        {
            return inter.ShowDialog();
        }

        public DialogResult ShowDialog(System.Windows.Forms.IWin32Window owner)
        {
            return inter.ShowDialog(owner);
        }

        public void Dispose()
        {
            inter.Dispose();
        }

        public Object GetLifetimeService()
        {
            return inter.GetLifetimeService();
        }

        public Object InitializeLifetimeService()
        {
            return inter.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(System.Type requestedType)
        {
            return inter.CreateObjRef(requestedType);
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


        public bool CreatePrompt
        {
            get
            {
                return inter.CreatePrompt;
            }
            set
            {
                inter.CreatePrompt = value;
            }
        }
        public bool OverwritePrompt
        {
            get
            {
                return inter.OverwritePrompt;
            }
            set
            {
                inter.OverwritePrompt = value;
            }
        }
        public bool AddExtension
        {
            get
            {
                return inter.AddExtension;
            }
            set
            {
                inter.AddExtension = value;
            }
        }
        public bool CheckFileExists
        {
            get
            {
                return inter.CheckFileExists;
            }
            set
            {
                inter.CheckFileExists = value;
            }
        }
        public bool CheckPathExists
        {
            get
            {
                return inter.CheckPathExists;
            }
            set
            {
                inter.CheckPathExists = value;
            }
        }
        public String DefaultExt
        {
            get
            {
                return inter.DefaultExt;
            }
            set
            {
                inter.DefaultExt = value;
            }
        }
        public bool DereferenceLinks
        {
            get
            {
                return inter.DereferenceLinks;
            }
            set
            {
                inter.DereferenceLinks = value;
            }
        }
        public String FileName
        {
            get
            {
                return inter.FileName;
            }
            set
            {
                inter.FileName = value;
            }
        }
        public System.String[] FileNames
        {
            get
            {
                return inter.FileNames;
            }
        }
        public String Filter
        {
            get
            {
                return inter.Filter;
            }
            set
            {
                inter.Filter = value;
            }
        }
        public Int32 FilterIndex
        {
            get
            {
                return inter.FilterIndex;
            }
            set
            {
                inter.FilterIndex = value;
            }
        }
        public String InitialDirectory
        {
            get
            {
                return inter.InitialDirectory;
            }
            set
            {
                inter.InitialDirectory = value;
            }
        }
        public bool RestoreDirectory
        {
            get
            {
                return inter.RestoreDirectory;
            }
            set
            {
                inter.RestoreDirectory = value;
            }
        }
        public bool ShowHelp
        {
            get
            {
                return inter.ShowHelp;
            }
            set
            {
                inter.ShowHelp = value;
            }
        }
        public bool SupportMultiDottedExtensions
        {
            get
            {
                return inter.SupportMultiDottedExtensions;
            }
            set
            {
                inter.SupportMultiDottedExtensions = value;
            }
        }
        public String Title
        {
            get
            {
                return inter.Title;
            }
            set
            {
                inter.Title = value;
            }
        }
        public bool ValidateNames
        {
            get
            {
                return inter.ValidateNames;
            }
            set
            {
                inter.ValidateNames = value;
            }
        }
        public System.Windows.Forms.FileDialogCustomPlacesCollection CustomPlaces
        {
            get
            {
                return inter.CustomPlaces;
            }
        }
        public bool AutoUpgradeEnabled
        {
            get
            {
                return inter.AutoUpgradeEnabled;
            }
            set
            {
                inter.AutoUpgradeEnabled = value;
            }
        }
        public object Tag
        {
            get
            {
                return inter.Tag;
            }
            set
            {
                inter.Tag = value;
            }
        }
        public ISite Site
        {
            get
            {
                return inter.Site;
            }
            set
            {
                inter.Site = value;
            }
        }
        public System.ComponentModel.IContainer Container
        {
            get
            {
                return inter.Container;
            }
        }
        public event CancelEventHandler FileOk;
        public event EventHandler HelpRequest;
        public event EventHandler Disposed;
        private void EventAssotiation()
        {
            inter.FileOk += FileOk;
            inter.HelpRequest += HelpRequest;
            inter.Disposed += Disposed;
        }
    }
}
