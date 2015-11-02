using CsQuery;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        public static int Floor(this double d)
        {
            return Math.Floor(d)._Cast<int>();
        }
        public static int Ceiling(this double d)
        {
            return ((int)Math.Ceiling(d));
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
                if (index >= str.Length)
                    return indexes;
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        public static String SplitAndGet(this String s, int Index, params char[] chars)
        {
            String[] t = s.Split(chars);
            if (Index >= t.Length)
                return null;
            return t[Index];
        }
        public static String SplitAndGet(this String s, int Index, params String[] str)
        {
            String[] t = s.Split(str, StringSplitOptions.None);
            if (Index >= t.Length)
                return null;
            return t[Index];
        }
        #endregion

        #region String[]
        public static bool Contains(this String[] arr, String str)
        {
            return arr.AsQueryable().Contains<string>(str);
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





        public static object GetAtInvoke(this ListBox self, int i)
        {
            if (self.InvokeRequired)
                return self.Invoke((Func<object>)delegate { return self.GetAtInvoke(i); });
            else
            {
                if (i >= self.Items.Count)
                    return null;
                return self.Items[i];
            }

        }

        public static int GetItemsCountInvoke(this ListBox self)
        {
            if (self.InvokeRequired)
                return (int)self.Invoke((Func<int>)delegate { return self.GetItemsCountInvoke(); });
            else
            {
                return self.Items.Count;
            }

        }

        public static void RemoveAtInvoke(this ListBox self, int i)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.GetAtInvoke(i); });
            else
            {
                if (i < self.Items.Count)
                    self.Items.RemoveAt(i);
            }
        }


        public static void SwapInvoke(this ListBox self, int item1, int item2)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.SwapInvoke(item1, item2); });
            else
            {
                if (item1 < self.Items.Count && item2 < self.Items.Count)
                {
                    object t = self.Items[item1];
                    self.Items[item1] = self.Items[item2];
                    self.Items[item2] = t;
                }

            }
        }
        #endregion

        #region DataGridView
        public static void SwapInvoke(this DataGridView self, int c1, int r1, int c2, int r2)
        {
            if (self.InvokeRequired)
            {
                self.Invoke((MethodInvoker)delegate { self.SwapInvoke(c1, r1, c2, r2); });
            }
            else
                if (c1 < self.Columns.Count && c2 < self.Columns.Count && r1 < self.Rows.Count && r2 < self.Rows.Count)
            {
                object o1 = self[c1, r1].Value;
                self[c1, r1].Value = self[c2, r2].Value;
                self[c2, r2].Value = o1;
            }
        }

        public static int GetColumnCountInvoke(this DataGridView self)
        {
            if (self.InvokeRequired)
                return (int)self.Invoke((Func<int>)delegate { return self.GetColumnCountInvoke(); });
            else
                return self.Columns.Count;
        }
        public static int GetRowCountInvoke(this DataGridView self)
        {
            if (self.InvokeRequired)
                return (int)self.Invoke((Func<int>)delegate { return self.GetRowCountInvoke(); });
            else
                return self.Rows.Count;
        }

        public static object GetAtInvoke(this DataGridView self, int c, int r)
        {
            if (self.InvokeRequired)
                return self.Invoke((Func<object>)delegate { return self.GetAtInvoke(c, r); });
            else
                return self[c, r].Value;
        }
        public static void SetAtInvoke(this DataGridView self, int c, int r, object o)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.SetAtInvoke(c, r, o); });
            else
                self[c, r].Value = o;
        }

        public static void SetColumNameInvoke(this DataGridView self, int Column, String Name)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.SetColumNameInvoke(Column, Name); });
            else
            {
                self.Columns[Column].HeaderText = Name;
            }
        }

        public static int GetNotNullElementInColumnInvoke(this DataGridView self, int Column)
        {
            if (self.InvokeRequired)
                return (int)self.Invoke((Func<int>)delegate { return self.GetNotNullElementInColumnInvoke(Column); });
            else
            {
                if (Column < self.Columns.Count)
                {
                    int c = 0;
                    for (int i = 0; i < self.Rows.Count; i++)
                        if (self[Column, i].Value != null)
                            c++;
                    return c;
                }
                return 0;
            }
        }
        public static void RemoveEmptyRowInvoke(this DataGridView self)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.RemoveEmptyRowInvoke(); });
            else
            {
                List<DataGridViewRow> lr = new List<DataGridViewRow>();
                for (int i = 0; i < self.Rows.Count; i++)
                {
                    bool allNull = true;
                    for (int j = 0; j < self.Columns.Count; j++)
                    {
                        if (self[j, i].Value != null)
                        {
                            allNull = false;
                        }
                    }
                    if (allNull)
                        lr.Add(self.Rows[i]);
                }
                foreach (DataGridViewRow r in lr)
                {
                    self.Rows.Remove(r);
                }
            }
        }


        public static void ShiftUpInvoke(this DataGridView self, int Column, int RemovePosition)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.ShiftUpInvoke(Column, RemovePosition); });
            else
            {
                for (int i = RemovePosition; i < self.Rows.Count - 1; i++)
                    self[Column, i].Value = self[Column, i + 1].Value;
                self[Column, self.Rows.Count - 1].Value = null;
            }
        }
        public static void ShiftDownInvoke(this DataGridView self, int Column, int Position)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.ShiftDownInvoke(Column, Position); });
            else
            {
                for (int i = self.Rows.Count - 1; i > Position; i--)
                    self[Column, i].Value = self[Column, i - 1].Value;
                self[Column, Position].Value = null;
            }
        }





        #endregion

        #region object
        public static T _Cast<T>(this object o)
        {
            return (T)o;
        }
        #endregion

        #region Point
        public static bool InPoligon(this Point[] Points, Point p)
        {
            int max_point = Points.Length - 1;
            float total_angle = GetAngle(Points[max_point].X, Points[max_point].Y, p.X, p.Y, Points[0].X, Points[0].Y);


            for (int i = 0; i < max_point; i++)
            {
                total_angle += GetAngle(Points[i].X, Points[i].Y, p.X, p.Y, Points[i + 1].X, Points[i + 1].Y);
            }

            return (Math.Abs(total_angle) > 0.000001);
        }

        private static float GetAngle(float Ax, float Ay, float Bx, float By, float Cx, float Cy)
        {
            float dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy);
            float cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);
            return (float)Math.Atan2(cross_product, dot_product);
        }
        private static float DotProduct(float Ax, float Ay, float Bx, float By, float Cx, float Cy)
        {
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;
            return (BAx * BCx + BAy * BCy);
        }
        private static float CrossProductLength(float Ax, float Ay, float Bx, float By, float Cx, float Cy)
        {
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;
            return (BAx * BCy - BAy * BCx);
        }
        #endregion

        #region Uri
        public static void Append(this Uri self, String s)
        {
            self = new Uri(self, s);
        }
        #endregion

        #region Form
        public static void CloseInvoke(this Form self)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.CloseInvoke(); });
            else
                self.Close();
            
        }
        public static void HideInvoke(this Form self)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate {
                    self.HideInvoke();
                });
            else
                self.Hide();

        }
        public static void ShowInvoke(this Form self)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.ShowInvoke(); });
            else
                self.Show();

        }
        public static void BringToFrontInvoke(this Form self)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.BringToFrontInvoke(); });
            else
                self.BringToFront();

        }
        public static void SetWindowStateInvoke(this Form self,FormWindowState fws)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.WindowState = fws; });
            else
                self.WindowState = fws;

        }
        public static void SetTopMostInvoke(this Form self, bool TopMost)
        {
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.TopMost = TopMost; });
            else
                self.TopMost = TopMost;

        }
        #endregion

        #region Enum
        public static String ToStringSpace(this Enum e)
        {
            return e.ToString().Replace("_", " ");
        }
        public static String ToStringReplace(this Enum e, String find, String replace)
        {
            return e.ToString().Replace(find, replace);
        }
        public static String ToString(this Enum e)
        {
            return e.ToString();
        }

        public static bool IsAn<T>(this Enum e)
        {
            if (e is T)
                return true;
            return false;
        }
        #endregion


        #region DEMO

        #endregion
    }
}
