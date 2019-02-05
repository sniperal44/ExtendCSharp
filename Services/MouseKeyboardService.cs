using ExtendCSharp.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class MouseService : IService
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// Permette di lanciare un evento del mouse in una determinata cordinata
        /// </summary>
        /// <param name="me">Evento</param>
        /// <param name="X">Cordinata X</param>
        /// <param name="Y">Cordinata Y</param>
        /// <param name="BackOldPosition">True - alla fine, re-imposta le cordinate del mouse prima del click</param>
        /// <param name="WheelDelta">Se l'evento riguarda la wheel impostare il delta dello spostamento, altrimenti lasciare 0</param>
        public void DoEvent(MyMouseEvent me, int X, int Y,bool BackOldPosition = true,int WheelDelta=0)
        {
            Point OldP = Cursor.Position;
            if (me.Event==MyMouseEvent.MouseEvent.Click)
            {
                me.Event = MyMouseEvent.MouseEvent.Down;
                mouse_event((int)me.ToInternalMouseEvent().Value, X, Y, WheelDelta, 0);
                me.Event = MyMouseEvent.MouseEvent.Up;
                mouse_event((int)me.ToInternalMouseEvent().Value, X, Y, WheelDelta, 0);
            }
            else
            {
                MouseEventInternal? i = me.ToInternalMouseEvent();
                if (i != null)
                    mouse_event((int)i.Value, X, Y, WheelDelta, 0);
            }
            if (BackOldPosition)
                Cursor.Position = OldP;

        }
        /// <summary>
        /// Permette di lanciare un evento del mouse in una determinata cordinata
        /// </summary>
        /// <param name="me">Evento</param>
        /// <param name="position">Posizione del click</param>
        /// <param name="BackOldPosition">True - alla fine, re-imposta le cordinate del mouse prima del click</param>
        /// <param name="WheelDelta">Se l'evento riguarda la wheel impostare il delta dello spostamento, altrimenti lasciare 0</param>
        public void DoEvent(MyMouseEvent me, Point position, bool BackOldPosition = true, int WheelDelta = 0)
        {
            DoEvent(me, position.X, position.Y, BackOldPosition, WheelDelta);
        }
        public void DoEvent(MyMouseEvent me, int WheelDelta = 0)
        {
            int X = Cursor.Position.X, Y = Cursor.Position.Y;
            if (me.Event == MyMouseEvent.MouseEvent.Click)
            {
                me.Event = MyMouseEvent.MouseEvent.Down;
                mouse_event((int)me.ToInternalMouseEvent().Value, X, Y, WheelDelta, 0);
                me.Event = MyMouseEvent.MouseEvent.Up;
                mouse_event((int)me.ToInternalMouseEvent().Value, X, Y, WheelDelta, 0);
            }
            else
            {
                MouseEventInternal? i = me.ToInternalMouseEvent();
                if (i != null)
                    mouse_event((int)i.Value, X, Y, WheelDelta, 0);
            }
        }

    }
    public class KeyboardService : IService
    {
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag

        [DllImport("user32.dll")]
        private static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);


        public MyKeyboardEvent GetKeyState(int vKey)
        {
            return new MyKeyboardEvent((GetAsyncKeyStateStatus)GetAsyncKeyState(vKey));
        }
        public MyKeyboardEvent GetKeyState(Keys vKey)
        {
            return new MyKeyboardEvent((GetAsyncKeyStateStatus)GetAsyncKeyState(vKey));
        }


        public void SendKeyEvent(MyKeyboardEvent ke, Keys k)
        {
            
            if (k == Keys.LMenu)
            {
                //keybd_event(0x12, 0xB8, a, 0);
                keybd_event(18, 0, (uint)ke.ToSendKeyEventInternal().Value, 0);
            }
            else
            {
                keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | (uint)ke.ToSendKeyEventInternal().Value, 0);
            }
        }
        public void SendKeyUpDown(Keys k)
        {
            MyKeyboardEvent ke = new MyKeyboardEvent(MyKeyboardEvent.KeyStatus.Up);
            SendKeyEvent(ke, k);
            ke.KeyStat = MyKeyboardEvent.KeyStatus.Down;
            SendKeyEvent(ke, k);
        }
        public void SendKeyDownUp(Keys k)
        {
            MyKeyboardEvent ke = new MyKeyboardEvent(MyKeyboardEvent.KeyStatus.Down);
            SendKeyEvent(ke, k);
            ke.KeyStat = MyKeyboardEvent.KeyStatus.Up;
            SendKeyEvent(ke, k);
        }

    }


    public class MyMouseEvent
    {
        public enum MouseButton
        {
            Left,
            Middle,
            Right,
            None
        }
        public enum MouseEvent
        {
            Move,
            Click,
            Down,
            Up,
            Absolute,
            Wheel,
            None,
        }
        private enum MouseButtonShift
        {
            Left=0,
            Right=2,
            Middle=4,
        }
        private enum MouseEventNumer
        {
            Down=2,
            Up=4
        }

        public MouseButton Button = MouseButton.None;
        public MouseEvent Event = MouseEvent.None;

        public MyMouseEvent(MouseButton Button, MouseEvent Event)
        {
            this.Button = Button;
            this.Event = Event;
        }
        public MyMouseEvent(MouseEventInternal ev)
        {
            FromInternalMouseEvent(ev);
        }
        public MyMouseEvent(MouseMessagesInternal ev)
        {
            FromInternalMouseMessage(ev);
        }


        public MouseEventInternal? ToInternalMouseEvent()
        {
            if (Event == MouseEvent.Move)
                return MouseEventInternal.Move;
            else if (Event == MouseEvent.Absolute)
                return MouseEventInternal.Absolute;
            else if (Event == MouseEvent.Wheel)
                return MouseEventInternal.Wheel;
            else if (Button==MouseButton.None || Event==MouseEvent.Click || Event==MouseEvent.None)
                return null;

            MouseButtonShift shift = Button == MouseButton.Right ? MouseButtonShift.Right : Button == MouseButton.Middle ? MouseButtonShift.Middle : MouseButtonShift.Left;
            MouseEventNumer ev = Event == MouseEvent.Down ? MouseEventNumer.Down : MouseEventNumer.Up;
            
            return (MouseEventInternal)((int)ev << (int)shift);
        }
        public void FromInternalMouseEvent(MouseEventInternal ev)
        {
            if (ev == MouseEventInternal.Wheel)
            {
                Button = MouseButton.None;
                Event = MouseEvent.Wheel;
            }
            else if (ev == MouseEventInternal.Absolute)
            {
                Button = MouseButton.None;
                Event = MouseEvent.Absolute;
            }
            else if (ev == MouseEventInternal.Move)
            {
                Button = MouseButton.None;
                Event = MouseEvent.Move;
            }
            else
            {
                int t = (int)ev;
                if (t > 10)
                {
                    t = t >> (int)MouseButtonShift.Middle;
                    Button = MouseButton.Middle;
                }
                else if (t > 4)
                {
                    t = t >> (int)MouseButtonShift.Right;
                    Button = MouseButton.Right;
                }
                else
                    Button = MouseButton.Left;

                if(t==(int)MouseEventNumer.Up)
                    Event = MouseEvent.Up;
                else if (t == (int)MouseEventNumer.Down)
                    Event = MouseEvent.Down;
            }
        }

        public MouseMessagesInternal? ToInternalMouseMessage()
        {
            
            if (Event == MouseEvent.Move)
                return MouseMessagesInternal.MouseMove;
            else if (Event == MouseEvent.Wheel)
                return MouseMessagesInternal.MouseWheel;
            else if (Button == MouseButton.None || Event == MouseEvent.None || Event == MouseEvent.Absolute)
                return null;
            else
            {
                int btt = Button == MouseButton.Left ? 0x0201 : Button == MouseButton.Right ?  0x0204 : 0x0207;
                int ev = Event == MouseEvent.Down ? 0 : Event == MouseEvent.Up ?1:2;
                return (MouseMessagesInternal)(btt + ev);
            }
        }
        public void FromInternalMouseMessage(MouseMessagesInternal ev)
        {
            if (ev == MouseMessagesInternal.MouseMove)
            {
                Button = MouseButton.None;
                Event = MouseEvent.Move;
            }
            else if (ev == MouseMessagesInternal.MouseWheel)
            {
                Button = MouseButton.None;
                Event = MouseEvent.Wheel;
            }
            else
            {
                int t = (int)ev - 0x0201;
                int btt = t / 3;
                int e = t % 3;

                Event = e == 0 ? MouseEvent.Down : e == 1 ? MouseEvent.Up : MouseEvent.Click;
                Button = btt == 0 ? MouseButton.Left : btt == 1 ? MouseButton.Right : MouseButton.Middle;
            }
        }


        public override string ToString()
        {
            return Button.ToString()+" - "+ Event.ToString();
        }
    }


    public enum MouseEventInternal
    {
        Move = 0x01,
        LeftDown = 0x02,
        LeftUp = 0x04,
        RightDown = 0x08,
        RightUp = 0x10,
        MiddleDown = 0x20,
        MiddleUp = 0x40,
        Absolute = 0x8000,
        Wheel=0x0800
    }
    public enum MouseMessagesInternal
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
        MButtonClick = 0x0209,

        MouseWheel = 0x020A,
    }





    public class MyKeyboardEvent
    {
        public enum KeyStatus
        {
            Up,
            Down,
            Press,
            None
        }

        public KeyStatus KeyStat = KeyStatus.None;

        public MyKeyboardEvent(KeyStatus KeyStat)
        {
            this.KeyStat = KeyStat;
        }
        public MyKeyboardEvent(SendKeyEventInternal ev)
        {
            FromSendKeyEventInternal(ev);
        }
        public MyKeyboardEvent(HookKeyStatusInternal ev)
        {
            FromHookKeyStatusInternal(ev);
        }
        public MyKeyboardEvent(GetAsyncKeyStateStatus ev)
        {
            FromGetAsyncKeyStateStatus(ev);
        }


        public SendKeyEventInternal? ToSendKeyEventInternal()
        {
            if (KeyStat == KeyStatus.Down)
                return SendKeyEventInternal.Down;
            else if (KeyStat == KeyStatus.Up)
                return SendKeyEventInternal.Up;
            return null;

        }
        public void FromSendKeyEventInternal(SendKeyEventInternal ev)
        {
            KeyStat = ev == SendKeyEventInternal.Down ? KeyStatus.Down : ev == SendKeyEventInternal.Up ? KeyStatus.Up : KeyStatus.None;
        }


        public HookKeyStatusInternal? ToHookKeyStatusInternal()
        {
            if (KeyStat == KeyStatus.Down)
                return HookKeyStatusInternal.WM_KEYDOWN;
            else if (KeyStat == KeyStatus.Up)
                return HookKeyStatusInternal.WM_KEYUP;
            return null;
        }
        public void FromHookKeyStatusInternal(HookKeyStatusInternal ev)
        {
            KeyStat = ev == HookKeyStatusInternal.WM_KEYDOWN || ev == HookKeyStatusInternal.WM_SYSKEYDOWN ? KeyStatus.Down : ev == HookKeyStatusInternal.WM_KEYUP ||  ev== HookKeyStatusInternal.WM_SYSKEYUP ? KeyStatus.Up : KeyStatus.None;
        }


        public GetAsyncKeyStateStatus? ToGetAsyncKeyStateStatus()
        {
            if (KeyStat == KeyStatus.Down)
                return GetAsyncKeyStateStatus.KeyDown;
            else if (KeyStat == KeyStatus.Press)
                return GetAsyncKeyStateStatus.KeyPressed;
            else if (KeyStat == KeyStatus.None)
                return GetAsyncKeyStateStatus.KeyNotPressed;
            return null;
        }
        public void FromGetAsyncKeyStateStatus(GetAsyncKeyStateStatus ev)
        {
            KeyStat = ev == GetAsyncKeyStateStatus.KeyDown ? KeyStatus.Down : ev == GetAsyncKeyStateStatus.KeyPressed ? KeyStatus.Press : KeyStatus.None;
        }


        public override string ToString()
        {
            return KeyStat.ToString();
        }
    }


    public enum SendKeyEventInternal
    {
        Down = 0x0000,
        Up = 0x0002
    }
    public enum HookKeyStatusInternal
    {
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_SYSKEYDOWN=0x0104,
        WM_SYSKEYUP= 0x0105,
    }
    public enum GetAsyncKeyStateStatus
    {
        KeyDown= -32767,
        KeyPressed= -32768,
        KeyNotPressed=0
    }


}
