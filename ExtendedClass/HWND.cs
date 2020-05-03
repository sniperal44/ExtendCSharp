using ExtendCSharp.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public bool MoveWindow( int X, int Y, int nWidth, int nHeight, bool bRepaint)
        {
            return MoveWindow(this, X, Y, nWidth, nHeight, bRepaint);
        }
    }
}
