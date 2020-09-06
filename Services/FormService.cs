using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class FormService : IService
    {
        Dictionary<Form, ThreadPlus> ListThread = new Dictionary<Form, ThreadPlus>();

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



        async public Task StartFormThread(Func<Form> FunzioneCreazione)
        {
            CancellationTokenSource cs = new CancellationTokenSource();
            CancellationToken ct = cs.Token;

            Task ts=Task.Factory.StartNew(() =>
            {
                Form f = FunzioneCreazione();
                f.ShowDialog();
            },ct);

            await ts;

            /*ThreadPlus t = new ThreadPlus((object CurrentThread) =>
            {
                bool Finito = false;
                Form f = FunzioneCreazione();
                ListThread.Add(f, (ThreadPlus)CurrentThread);

                f.FormClosing += (object sender, FormClosingEventArgs e) =>
                {
                    f.Dispose();
                    Finito = true;
                };
                

                f.Show();
                
                //TODO: implementare i task per la gestione asincrona della cancellazione
                // creare un task cancellabile
                // richiamare l'await di quel task
                // richiamare la cancellazione del task al richiamo de FormClosed
                // ( in modo da togliere il ciclo while )
                while (!Finito)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                

            });
            t.Start(t);*/

        }

       

        public void StopThread(Form f)
        {
            if (f == null)
                return;

            if(ListThread.ContainsKey(f))
            {
                try
                {
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
                    kvp.Value.Abort();
                }
                catch (Exception)
                {

                }
            });
        }

    }
}
