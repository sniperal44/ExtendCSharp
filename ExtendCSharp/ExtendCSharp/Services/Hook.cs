using ExtendCSharp.Interfaces;
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
    static class HookService
    {
        public delegate int HookProcInterno(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProcInterno lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }



    public interface Hook
    {
        void Enable();
        bool Disable();

    }
    public class HookMouse : Hook, IService
    {
        public delegate void HookProcMouse(int nCode, MyMouseEvent wParam, IntPtr lParam, ref bool Suppress);
        public event HookProcMouse EventDispatcher = null;
        private int Hook = 0;
        private HookProcInterno HPI;

        private int Proc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool Suppress = false;
            if (EventDispatcher != null)
                EventDispatcher(nCode, new MyMouseEvent((MouseMessagesInternal)wParam), lParam, ref Suppress);

            return Suppress ? 1 : CallNextHookEx(Hook, nCode, wParam, lParam);
        }

        public void Enable()
        {
            if (Hook == 0)
            {
                HPI = new HookProcInterno(Proc);
                Hook = SetWindowsHookEx((int)HookTypes.WH_MOUSE_LL, HPI, (IntPtr)0, 0);
            }
        }
        public bool Disable()
        {
            return HookService.UnhookWindowsHookEx(Hook);
        }
    }
    public class HookKeyboard : Hook, IService
    {
        public delegate void HookProcKeyboard(int nCode, MyKeyboardEvent wParam, Keys key, ref bool Suppress);
        public event HookProcKeyboard EventDispatcher = null;
        private int Hook = 0;
        private HookProcInterno HPI;

        private int Proc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //Log.Log.AddLog("nCode: " + nCode + " - wParam" + wParam + " - lParam" + lParam+" - marshal"+ Marshal.ReadInt32(lParam));
          
            bool Suppress = false;
            if (EventDispatcher != null)
                EventDispatcher(nCode, new MyKeyboardEvent((HookKeyStatusInternal)wParam),(Keys)Marshal.ReadInt32(lParam), ref Suppress);

            return Suppress ? 1 : CallNextHookEx(Hook, nCode, wParam, lParam);
        }

        public void Enable()
        {
            if (Hook == 0)
            {
                HPI = new HookProcInterno(Proc);
                Hook = SetWindowsHookEx((int)HookTypes.WH_KEYBOARD_LL, HPI, (IntPtr)0, 0);
            }
        }
        public bool Disable()
        {
            return HookService.UnhookWindowsHookEx(Hook);
        }
    }



    public class HookManager : IService
    {
        public List<Hook> Hooks;
        public HookManager()
        {
            Hooks = new List<Hook>();
        }
        public HookManager(params Hook[] Hooks)
        {
            this.Hooks = new List<Hook>();
            if (Hooks != null)
                this.Hooks.AddRange(Hooks);
        }


        public void EnableAll()
        {
            Hooks.ForEach(a => a.Enable());
        }
        public void DisableAll()
        {
            Hooks.ForEach(a => a.Disable());
        }
    }


    public enum HookTypes
    {
        WH_MOUSE_LL = 14,
        WH_KEYBOARD_LL = 13
    }
}
