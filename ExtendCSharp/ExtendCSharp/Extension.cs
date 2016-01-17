using CsQuery;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static bool IsFloat(this String d)
        {
            float n;
            return float.TryParse(d, out n);
        }
        public static float ParseFloat(this String d)
        {
            return float.Parse(d);
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

        public static String SplitAndGetLast(this String s, params char[] chars)
        {
            String[] t = s.Split(chars);
            return t.Last();
        }
        public static String SplitAndGetLast(this String s, params String[] str)
        {
            String[] t = s.Split(str, StringSplitOptions.None);
            return t.Last();
        }

        public static String SplitAndGetFirst(this String s, params char[] chars)
        {
            String[] t = s.Split(chars);
            return t.First();
        }
        public static String SplitAndGetFirst(this String s,  params String[] str)
        {
            String[] t = s.Split(str, StringSplitOptions.None);
            return t.First();
        }


        public static String RemoveLeft(this String s,params String[] str)
        {
            bool repeat = false;
            String st = s;
            do
            {
                foreach (String ss in str)
                    if (st.StartsWith(ss))
                    {
                        st = st.Remove(0, ss.Length);
                        repeat = true;
                    }
            }
            while (repeat == true && str.Length > 1);
            return st;
        }
        public static String RemoveRight(this String s, params String[] str)
        {
            bool repeat = false;
            String st = s;
            do
            {
                foreach (String ss in str)
                    if (st.EndsWith(ss))
                    {
                        st.Remove(st.LastIndexOf(ss));
                        repeat = true;
                    } 
            }
            while (repeat == true && str.Length > 1);
            return st;
        }

        public static String OneCharEnd(this String s,char c)
        {
            s.TrimEnd(c);
            return s + c;
        }
        public static String OneCharStart(this String s, char c)
        {
            s.TrimStart(c);
            return c+s;
        }

        #endregion

        #region String[]

        public static bool Contains(this String[] arr, String str)
        {
            return arr.AsQueryable().Contains<string>(str);
        }

        #endregion

        #region byte[]

        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();

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
        public static void AppendTextInvoke(this Control t, string s)
        {
            if (t.InvokeRequired)
                t.Invoke((MethodInvoker)delegate { t.AppendTextInvoke(s); });
            else
                t.Text = t.Text+s;
        }

        public static void SetEnableInvoke(this Control t, bool b)
        {
            if (t.InvokeRequired)
                t.Invoke((MethodInvoker)delegate { t.SetEnableInvoke(b); });
            else
                t.Enabled = b;
        }
        public static void SetVisibleInvoke(this Control t, bool b)
        {
            if (t.InvokeRequired)
                t.Invoke((MethodInvoker)delegate { t.SetVisibleInvoke(b); });
            else
                t.Visible = b;
        }


        public static Control SetSize(this Control t, int Width, int Height)
        {
            t.Size = new System.Drawing.Size(Width, Height);
            return t;
        }



        public static List<Control> GetControl(this Control Control, bool TuttiILivelli)
        {
            List<Control> temp = new List<Control>();

            if (TuttiILivelli)
            {
                foreach (Control cont in Control.Controls)
                {
                    temp.AddRange(GetControl(cont, true));
                    temp.Add(cont);
                }
                return temp;
            }
            else
            {
                foreach (Control cont in Control.Controls)
                    temp.Add(cont);
                return temp;
            }
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
        public static void Replace(this Control.ControlCollection self, Control ToRemove, Control ToAdd)
        {
            ToAdd.Size = ToRemove.Size;
            ToAdd.Location = ToRemove.Location;
            self.Remove(ToRemove);
            self.Add(ToAdd);
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



        public static void ClearInvoke(this ListBox self)
        {

            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.ClearInvoke(); });
            else
            {
                self.Items.Clear();
            }

        }
        public static void AddInvoke(this ListBox self, object obj)
        {
            
            if (self.InvokeRequired)
                self.Invoke((MethodInvoker)delegate { self.AddInvoke(obj); });
            else
            {
                self.Items.Add(obj);
            }

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

        public static bool InPoligon(this Point[] Points, Point p )
        {
            int i, j;
            bool c = false;
            for (i = 0, j = Points.Length - 1; i < Points.Length; j = i++)
            {
                if (((Points[i].Y > p.Y) != (Points[j].Y > p.Y)) &&
                 (p.X < (Points[j].X - Points[i].X) * (p.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X))
                    c = !c;
            }
            return c;
        }


      /*  public static bool InPoligon(this Point[] Points, Point p)
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
        }*/
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

        #region Graphics

        public static void DrawLines(this Graphics g, Pen pen, params Point[] p )
        {
            g.DrawLines(pen,p);
        }

        public static void DrawPolygon(this Graphics g, Pen pen, params Point[] p )
        {
            g.DrawPolygon(pen,p);
        }

        public static void DrawCircle(this Graphics g, Pen pen,float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush,float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,radius + radius, radius + radius);
        }

        #endregion

        #region MySQL

        public static Exception TryClose(this MySqlDataReader dr)
        {
            try
            {
                dr.Close();
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }
        public static Exception TryClose(this MySqlConnection dr)
        {
            try
            {
                dr.Close();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }


        public static object GetFieldValue(this MySqlDataReader dr,int i,Type tipo)
        {
            try
            {
                MethodInfo method = dr.GetType().GetMethod("GetFieldValue");
                method = method.MakeGenericMethod(tipo);
                return method.Invoke(dr, new object[] { i });
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        #endregion

        #region TextBox
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);


        public static void Caret(this TextBox dr,bool Visible)
        {
            if (Visible)
                ShowCaret(dr.Handle);
            else
                HideCaret(dr.Handle);
        }
        #endregion

        #region DEMO

        #endregion
    }
}
