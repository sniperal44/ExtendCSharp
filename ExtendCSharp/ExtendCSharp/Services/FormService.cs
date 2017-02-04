using ExtendCSharp.ExtendedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class FormService
    {
        static Dictionary<Form, ThreadPlus> ListThread = new Dictionary<Form, ThreadPlus>();

        public static void StartFormThread(Func<Form> FunzioneCreazione)
        {
            ThreadPlus t = new ThreadPlus((object CurrentThread) =>
            {
                Form f = FunzioneCreazione();
                ListThread.Add(f, (ThreadPlus)CurrentThread);

                f.ShowDialog();
            });
            t.Start(t);

        }
        public static void StopThread(Form f)
        {
            if(ListThread.ContainsKey(f))
            {
                try
                {
                    ListThread[f].Abort();
                }
                catch(Exception ex)
                {

                }
            }
        }


    }
}
