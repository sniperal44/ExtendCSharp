using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class FormService : IService
    {
        Dictionary<Form, ThreadPlus> ListThread = new Dictionary<Form, ThreadPlus>();

        public void StartFormThread(Func<Form> FunzioneCreazione)
        {
            
            ThreadPlus t = new ThreadPlus((object CurrentThread) =>
            {
                bool Finito = false;
                Form f = FunzioneCreazione();
                ListThread.Add(f, (ThreadPlus)CurrentThread);

                f.Show();
                f.FormClosed += (object sender, FormClosedEventArgs e)=>
                {
                    Finito = true;
                };
                while (!Finito)
                    Application.DoEvents();


            });
            t.Start(t);

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


    }
}
