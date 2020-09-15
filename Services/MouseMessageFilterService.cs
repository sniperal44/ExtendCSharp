using ExtendCSharp.Classes;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{

    /// <summary>
    /// Classe che permette di auto-registrarsi come gestore di messaggi dell'applicazione
    /// legge i messaggi e li filtra lanciando gli eventi corretti. 
    /// 
    /// Usata per ascoltare su una sola callback tutti gli eventi associati ( al mouse ) dell'applicazione
    /// </summary>
    public class MouseMessageFilterService : IMessageFilter, IDisposable,IService
    {



        public MouseMessageFilterService()
        {
        }

        public void Dispose()
        {
            StopFiltering();
        }

        #region IMessageFilter Members


        /// <summary>
        /// Funzione che filtra i messaggi in arrivo e lancia gli eventi corretti
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int) MouseMessagesInternal.MouseMove)
            {
                Point p = WinAPIHelper.GetPoint(m.LParam);
                MouseMove?.Invoke(null, new MouseEventArgs(WinAPIHelper.GetButtons(m.WParam), 0, p.X, p.Y, 0));
            }
            else if (m.Msg == (int)MouseMessagesInternal.LButtonDown)
            {
                Point p = WinAPIHelper.GetPoint(m.LParam);
                MouseDown?.Invoke(null, new MouseEventArgs(WinAPIHelper.GetButtons(m.WParam), 0, p.X, p.Y, 0));
            }
            else if (m.Msg == (int)MouseMessagesInternal.LButtonUp)
            {
                Point p = WinAPIHelper.GetPoint(m.LParam);
                MouseUp?.Invoke(null, new MouseEventArgs(WinAPIHelper.GetButtons(m.WParam), 0, p.X, p.Y, 0));
            }
            //TODO: implementare gli altri eventi
            return false;
        }

        
        #endregion


  

        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;

        public void StartFiltering()
        {
            StopFiltering();
            Application.AddMessageFilter(this);
        }

        public void StopFiltering()
        {
            Application.RemoveMessageFilter(this);
        }
        
    }
}
