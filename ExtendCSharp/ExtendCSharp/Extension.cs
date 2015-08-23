using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
    public static class Extension
    {        
        #region NUMBER
        public static bool IsInteger(this double d)
        {
            return (d - (int)d) == 0;
        }
        public static double Sqrt(this double d)
        {
            return Math.Sqrt(d);
        }
        public static double Sqrt(this int d)
        {
            return Math.Sqrt(d);
        }
        #endregion

        #region String
        public static bool IsInt(this String d)
        {
            int n;
            return int.TryParse(d, out n);
        }
        public static int ParseInt(this String d)
        {
            return int.Parse(d);
        }
        public static List<int> AllIndexesOf(this String str, string value)
        {
            List<int> indexes = new List<int>();
            if (String.IsNullOrEmpty(value))
                return indexes;
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        #endregion

        #region Control
        public static void SetTextInvoke(this Control t, string s)
        {
            if (t.InvokeRequired)
                t.Invoke((MethodInvoker)delegate { t.SetTextInvoke(s); });
            else
                t.Text = s;
        }
        public static void SetEnableInvoke(this Control t, bool b)
        {
            if (t.InvokeRequired)
                t.Invoke((MethodInvoker)delegate { t.SetEnableInvoke(b); });
            else
                t.Enabled = b;
        }

        public static Control SetSize(this Control t, int Width, int Height)
        {
            t.Size = new System.Drawing.Size(Width, Height);
            return t;
        }
        #endregion

        #region Control.ControlCollection
        public static void AddVertical(this Control.ControlCollection self, Control c)
        {
            int y = 0;
            foreach (Control cc in self)
            {
                if (cc.Location.Y + cc.Height > y)
                    y = cc.Location.Y + cc.Height;
            }
            c.Location = new System.Drawing.Point(0, y);
            self.Add(c);
        }
        #endregion

        #region FileInfo
        public static string GetFileNameWithoutExtension(this FileInfo self)
        {
            return Path.GetFileNameWithoutExtension(self.Name);
        }
        #endregion

        #region ProgressBar
        public static void SetValueInvoke(this ProgressBar p, int Value)
        {
            if (p.InvokeRequired)
                p.Invoke((MethodInvoker)delegate { p.SetValueInvoke(Value); });
            else
                p.Value = Value;
        }
        #endregion

        #region ListBox.ObjectCollection
        public static void AddUnique(this ListBox.ObjectCollection self, object obj)
        {
            foreach (object o in self)
            {
                if (o.Equals(obj))
                    return;
            }
            self.Add(obj);
        }
        #endregion

        #region object
        public static T Cast<T>(this object o)
        {
            return (T)o;
        }
        #endregion







        #region DEMO

        #endregion
    }
}
