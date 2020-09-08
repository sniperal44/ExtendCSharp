using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class FormService : IService
    {
        Dictionary<Form, ThreadPlus> _ListThread = new Dictionary<Form, ThreadPlus>();
        Dictionary<Form, ThreadPlus> ListThread {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (_ListThread == null)
                    _ListThread = new Dictionary<Form, ThreadPlus>();

                return _ListThread;
            } 
        }


        /// <summary>
        /// Permette di aprire un dialog
        /// </summary>
        /// <param name="FunzioneCreazione"> funzione che ritorna il forma da visualizzare. Usare la sintassi:
        /// <para /> ()=>{ return new MyForm();}
        /// </param>
        /// <param name="Invoker"></param>
        public void ShowDialog(Func<Form> FunzioneCreazione,Control Invoker)
        {
            if (Invoker == null)
                return;
            Invoker.BeginInvoke((MethodInvoker)delegate 
            {
                Form f = FunzioneCreazione();
                f.ShowDialog();
            });
        }



        async public Task StartFormInThread(Func<Form> FunzioneCreazione)
        {
            SemaphoreSlim ss = new SemaphoreSlim(0, 1);

            ThreadPlus t = new ThreadPlus((object CurrentThread) =>
            {     
                Form f = FunzioneCreazione();
                ListThread.Add(f, (ThreadPlus)CurrentThread);
                Application.Run(f);
                f.Dispose();
                ss.Release();
            });
            t.Start(t);

            await ss.WaitAsync();
        }


        public void StopThread(Form f)
        {
            if (f == null)
                return;

            if(ListThread.ContainsKey(f))
            {
                try
                {
                    f.CloseInvoke();
                    ListThread[f].Abort();
                }
                catch(Exception )
                {

                }
            }
        }

        public void StopAllThread()
        {
            ListThread.ForEach((kvp) =>
            {
                try
                {
                   kvp.Key.CloseInvoke();
                   kvp.Value.Abort();
                }
                catch (Exception)
                {

                }
            });
        }

    }
}
