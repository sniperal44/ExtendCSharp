using ExtendCSharp.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.ExtendedClass
{

    /// <summary>
    /// La classe permette di gestire un HWND partendo da un IntPtr come se fosse un oggetto
    /// </summary>
    public class HWND
    {
        #region Extern function
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion
        #region Static 

        /// <summary>
        /// Ritorna un HWND che punta alla console corrente
        /// </summary>
        /// <returns></returns>
        public static HWND GetCurrentConsole()
        {
            return new HWND(GetConsoleWindow());
        }

        #endregion

        #region Cast

        public static implicit operator IntPtr(HWND hwnd)
        {
            return hwnd._hwndPtr;
        }

        #endregion



        private IntPtr _hwndPtr;



        public HWND(IntPtr hwndPtr)
        {
            this._hwndPtr = hwndPtr;
        }



        /// <summary>
        /// Imposta lo status della finestra
        /// </summary>
        /// <param name="nCmdShow"></param>
        public void SetStatus(ShowWindowCommands nCmdShow)
        {
            ShowWindow(_hwndPtr, nCmdShow);
        }


        /// <summary>
        /// Imposta la finestra corrente come attiva 
        /// </summary>
        public void SetAsActiveWindow()
        {
            SetActiveWindow(this);
        }

        /// <summary>
        /// Ottiene il Point assoluto dello schermo rispetto al Point relativo alla finestra
        /// </summary>
        /// <param name="nCmdShow"></param>
        public Point ClientToScreen(Point lpPoint)
        {
            Point tmp = lpPoint.Clone();
            ClientToScreen(_hwndPtr, ref tmp);
            return tmp;
        }


        public IntPtr GetDC()
        {
            return GetDC(_hwndPtr);
        }
        public IntPtr GetThreadProcessId()
        {
            return GetWindowThreadProcessId(_hwndPtr, IntPtr.Zero);
        }

        public bool MoveWindow(int X, int Y, int nWidth, int nHeight, bool bRepaint)
        {
            return MoveWindow(this, X, Y, nWidth, nHeight, bRepaint);
        }
        public Rectangle getRect()
        {
            RECT rct = new RECT();
            GetWindowRect(_hwndPtr, ref rct);

            Rectangle r = new Rectangle(rct.Left, rct.Top, rct.Right - rct.Left, rct.Bottom - rct.Top);
            return r;
        }
        public int SetWindowStyle(WindowStyles styles)
        {
            return SetWindowLong(_hwndPtr, (int)WindowLongFlags.GWL_STYLE, (uint)styles);
        }
        public IntPtr SetWindowParent( IntPtr hWndNewParent)
        {
            return SetParent(_hwndPtr, hWndNewParent);
        }
        public IntPtr SetWindowParent(Control controlParent)
        {
            return SetParent(_hwndPtr, controlParent.Handle);
        }



        #region enums
        public enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }
        [Flags]
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,



            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,

            /*#if WINVER >= 0x0400

            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,

            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,

            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,

            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
            #endif 
            
            #if(WIN32WINNT >= 0x0500)

            WS_EX_LAYERED = 0x00080000,
            #endif 
            

            
            #if(WINVER >= 0x0500)

            WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
            WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
            #endif 
            

            
            #if(WIN32WINNT >= 0x0500)

            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000
            #endif 
            */
        
            #endregion
        }
    }
}
