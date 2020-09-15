using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Classes
{
    public static class WinAPIHelper
    {

        /*Point GetPoint(IntPtr _xy)
        {
            uint xy = unchecked(IntPtr.Size == 8 ? (uint)_xy.ToInt64() : (uint)_xy.ToInt32());
            int x = unchecked((short)xy);
            int y = unchecked((short)(xy >> 16));
            return new Point(x, y);
        }*/
        public static Point GetPoint(IntPtr lParam)
        {
            return new Point(GetInt(lParam));
        }
        public static MouseButtons GetButtons(IntPtr wParam)
        {
            MouseButtons buttons = MouseButtons.None;
            int btns = GetInt(wParam);
            if ((btns & MK_LBUTTON) != 0) buttons |= MouseButtons.Left;
            if ((btns & MK_RBUTTON) != 0) buttons |= MouseButtons.Right;
            return buttons;
        }
        static int GetInt(IntPtr ptr)
        {
            return IntPtr.Size == 8 ? unchecked((int)ptr.ToInt64()) : ptr.ToInt32();
        }
        const int MK_LBUTTON = 1;
        const int MK_RBUTTON = 2;
    }
}
