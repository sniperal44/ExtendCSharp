using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExtendCSharp.Services.HookService;

namespace ExtendCSharp.Services
{
    public static class MouseService
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>
        /// Permette di lanciare un evento del mouse in una determinata cordinata
        /// </summary>
        /// <param name="me">Evento</param>
        /// <param name="X">Cordinata X</param>
        /// <param name="Y">Cordinata Y</param>
        public static void DoEvent(MouseEvent me, int X, int Y)
        {
            mouse_event((int)me, X, Y, 0, 0);
        }



        /// <summary>
        /// Lancia un click nella posizione corrente del Mouse
        /// </summary>
        public static void LeftClick()
        {
            int X = Cursor.Position.X, Y = Cursor.Position.Y;
            mouse_event((int)MouseEvent.LeftDown, X, Y, 0, 0);
            mouse_event((int)MouseEvent.LeftUp, X, Y, 0, 0);
        }

        /// <summary>
        /// Lancia un click nelle cordinate passate
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="BackOldPosition">True - alla fine, re-imposta le cordinate del mouse prima del click</param>
        public static void LeftClick(int X, int Y, bool BackOldPosition = true)
        {
            int XR = Cursor.Position.X, YR = Cursor.Position.Y;
            mouse_event((int)MouseEvent.LeftDown, X, Y, 0, 0);
            mouse_event((int)MouseEvent.LeftUp, X, Y, 0, 0);
            Cursor.Position = new System.Drawing.Point(XR, YR);
        }

        /// <summary>
        /// Lancia un click nella posizione corrente del Mouse
        /// </summary>
        public static void RightClick()
        {
            int X = Cursor.Position.X, Y = Cursor.Position.Y;
            mouse_event((int)MouseEvent.RightDown, X, Y, 0, 0);
            mouse_event((int)MouseEvent.RightUp, X, Y, 0, 0);
        }

        /// <summary>
        /// Lancia un click nelle cordinate passate
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="BackOldPosition">True - alla fine, re-imposta le cordinate del mouse prima del click</param>
        public static void RightClick(int X, int Y, bool BackOldPosition = true)
        {
            int XR = Cursor.Position.X, YR = Cursor.Position.Y;
            mouse_event((int)MouseEvent.RightDown, X, Y, 0, 0);
            mouse_event((int)MouseEvent.RightUp, X, Y, 0, 0);
            Cursor.Position = new System.Drawing.Point(XR, YR);
        }


    }
    public static class KeyboardService
    {
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag

        [DllImport("user32.dll")]
        private static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);



        public static void SendKeyEvent(KeyEvent ke, Keys k)
        {   
            keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | (uint)ke, 0);
        }
        public static void SendKeyUpDown(Keys k)
        {
            SendKeyEvent(KeyEvent.Up, k);
            SendKeyEvent(KeyEvent.Down, k);
        }
        public static void SendKeyDownUp(Keys k)
        {
            SendKeyEvent(KeyEvent.Down, k);
            SendKeyEvent(KeyEvent.Up, k);
        }


    }

   
 

    public enum KeyEvent
    {
        Down = 0x0000,
        Up = 0x0002
    }
    public enum MouseEvent
    {
        LeftDown = 0x02,
        LeftUp = 0x04,
        RightDown = 0x08,
        RightUp = 0x10
    }

    public enum MouseButton
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_MBUTTON = 0x04,
    }

    public enum MouseMessages
    {
        MouseMove = 0x0200,

        LButtonDown = 0x0201,
        LButtonUp = 0x0202,
        LButtonClick = 0x0203,

        RButtonDown = 0x0204,
        RButtonUp = 0x0205,
        RButtonClick = 0x0206,

        MButtonDown = 0x0207,
        MButtonUp = 0x0208,
        MButtonClick = 0x0208,

        MouseWheel = 0x020A,
    }
    public enum KeyStatus
    {
        WM_KEYDOWN = 0x0100
    }




}
