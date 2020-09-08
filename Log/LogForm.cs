using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Services;
using System;
using System.Windows.Forms;

namespace ExtendCSharp.Log
{

    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            this.Show();
        }
        public void SetText(String s)
        {
            textBoxPlus1.Text=s;
        }
        public void AppendText(String s)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate { this.AppendText(s); });
            else
                textBoxPlus1.AppendText(s.TrimEnd('\r', '\n') + "\r\n");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxPlus1.Text = "";
        }

        private void textBoxPlus1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    public class Log
    {
        static ThreadPlus tp =null;
        static LogForm l;
        static bool Inizializzato = false;

        static String Pre = "";
        public static void AddIndent(String Indent= "\t")
        {
            Pre += "\t";
        }
        public static void RemoveIndent()
        {
            if (Pre.Length == 0)
                return;
            else if (Pre.Length == 1)
                Pre = "";
            else
                Pre = Pre.Substring(0, Pre.Length - 1);
        }

        public static void AddLog(String s)
        {
            if(tp==null)
            {
                tp = new ThreadPlus(RunLogForm);
                tp.Start();
            }
            
            while(!Inizializzato)
            {
                Application.DoEvents();
            }

            l.AppendText(Pre+s);
        }
        static Log()
        {
            
        }


        
        private static void RunLogForm()
        {
            FormService fs = ServicesManager.GetOrSet(() => { return new FormService(); });
            fs.StartFormInThread(() => {  l = new LogForm(); Inizializzato = true; return l; });

        }


        public static void Close()
        {
            FormService fs = ServicesManager.GetOrSet(() => { return new FormService(); });
            fs.StopThread(l);

        }
    }
}
