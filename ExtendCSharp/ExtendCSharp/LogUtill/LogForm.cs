using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendCSharp;

namespace ExtendCSharp.LogUtill
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
            textBoxPlus1.SetText(s);
        }
        public void AppendText(String s)
        {
            textBoxPlus1.AppendText(s.TrimEnd('\r','\n')+"\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxPlus1.SetText("");
        }

        private void textBoxPlus1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    public class Log
    {
        static LogForm l = new LogForm();
        public static void AddLog(String s)
        {
            l.AppendText(s);
        }
        static Log()
        {
            
        }
    }
}
