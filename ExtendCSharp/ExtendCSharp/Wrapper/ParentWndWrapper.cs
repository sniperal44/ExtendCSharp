using ExtendCSharp.ExtendedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Wrapper
{
    /// <summary>
    /// Questo Wrapper permette di incapsulare un IntPtr ad un HWND in un oggetto con interfaccia IWin32Window
    /// 
    /// da usare con form.show(IWin32WindowObj)
    /// </summary>
    public class ParentWndWrapper : IWin32Window
    {
        IntPtr m_Handle;
        public ParentWndWrapper(IntPtr pParent)
        {
            m_Handle = pParent;
        }
        public ParentWndWrapper(HWND pParent)
        {
            m_Handle = pParent;
        }

        #region IWin32Window Members
        public IntPtr Handle
        {
            get { return m_Handle; }
        }

        #endregion

    }
}
