using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return Math.Floor(d).Cast<int>();
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
        public static bool Contains(this String[] arr,String str)
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
        #endregion

        #region object
        public static T Cast<T>(this object o)
        {
            return (T)o;
        }
        #endregion

        #region WebBrowser





        public static void Navigate(this WebBrowser wb, String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            System.Text.Encoding a = System.Text.Encoding.UTF8;
            byte[] byte1 = a.GetBytes(PostData);
            wb.Navigate(Url, TargetFrame, byte1, AdditionalHeaders);
        }
        public static void Navigate(this WebBrowser wb, Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            System.Text.Encoding a = System.Text.Encoding.UTF8;
            byte[] byte1 = a.GetBytes(PostData);
            wb.Navigate(Url, TargetFrame, byte1, AdditionalHeaders);
        }




        public static void NavigateInvoke(this WebBrowser wb, String Url)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url); });
            else
                wb.Navigate(Url);
        }
        public static void NavigateInvoke(this WebBrowser wb, Uri Url)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url); });
            else
                wb.Navigate(Url);
        }
        public static void NavigateInvoke(this WebBrowser wb, String Url, bool NewWindow)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, NewWindow); });
            else
                wb.Navigate(Url, NewWindow);
        }
        public static void NavigateInvoke(this WebBrowser wb, Uri Url, bool NewWindow)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, NewWindow); });
            else
                wb.Navigate(Url, NewWindow);
        }
        public static void NavigateInvoke(this WebBrowser wb, String Url, string TargetFrame)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, TargetFrame); });
            else
                wb.Navigate(Url, TargetFrame);
        }
        public static void NavigateInvoke(this WebBrowser wb, Uri Url, string TargetFrame)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, TargetFrame); });
            else
                wb.Navigate(Url, TargetFrame);
        }
        public static void NavigateInvoke(this WebBrowser wb, String Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                wb.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);

        }
        public static void NavigateInvoke(this WebBrowser wb, Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                wb.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        public static void NavigateInvoke(this WebBrowser wb, String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url,TargetFrame, PostData, AdditionalHeaders); });
            else
                wb.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        public static void NavigateInvoke(this WebBrowser wb, Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            if (wb.InvokeRequired)
                wb.Invoke((MethodInvoker)delegate { wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                wb.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }


        public static void NavigateAndWait(this WebBrowser wb,String Url)
        {
            wb.NavigateInvoke(Url);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, Uri Url)
        {
            wb.NavigateInvoke(Url);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, String Url, bool NewWindow)
        {
            wb.NavigateInvoke(Url, NewWindow);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, Uri Url,bool NewWindow)
        {
            wb.NavigateInvoke(Url, NewWindow);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, String Url, string TargetFrame)
        {
            wb.NavigateInvoke(Url, TargetFrame);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, Uri Url, string TargetFrame)
        {
            wb.NavigateInvoke(Url, TargetFrame);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, String Url, string TargetFrame,byte[]PostData, String AdditionalHeaders)
        {
            wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            wb.WaitCompleteInvoke();

        }
        public static void NavigateAndWait(this WebBrowser wb, Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            wb.WaitCompleteInvoke();
        }
        public static void NavigateAndWait(this WebBrowser wb, Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            wb.NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            wb.WaitCompleteInvoke();
        }


        public static void WaitCompleteInvoke(this WebBrowser wb)
        {
            if(wb.InvokeRequired)
            {
                wb.Invoke((MethodInvoker) delegate { wb.WaitCompleteInvoke(); });
            }
            else
            {
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
            }
        }

        public static HtmlDocument DocumentInvoke(this WebBrowser wb)
        {
            if (wb.InvokeRequired)
            {
                return (HtmlDocument)wb.Invoke((Func < HtmlDocument >)delegate { return wb.DocumentInvoke(); });
            }
            else
            {
                return wb.Document;
            }
        }



        #endregion

        #region HtmlDocument
        public static List<HtmlElement> GetElementsByClass(this HtmlDocument self,String Class)
        {
            List<HtmlElement> t = new List<HtmlElement>();
            foreach (HtmlElement hel in self.All)
            {
                if (hel.GetAttribute("className").Split(' ', '\t').Contains(Class))
                    t.Add(hel);
            }
            return t;
        }
        public static List<HtmlElement> GetElementsByTagNameClass(this HtmlDocument self, String Class,String Tag)
        {
            List<HtmlElement> t = new List<HtmlElement>();
            foreach (HtmlElement hel in self.GetElementsByTagName(Tag))
            {
                if (hel.GetAttribute("className").Split(' ', '\t').Contains(Class))
                    t.Add(hel);
            }
            return t;
        }


        #endregion

        #region HtmlElement
        public static List<HtmlElement> GetElementsByClass(this HtmlElement self, String Class)
        {
            List<HtmlElement> t = new List<HtmlElement>();
            foreach (HtmlElement hel in self.All)
            {
                if (hel.GetAttribute("className").Split(' ', '\t').Contains(Class))
                    t.Add(hel);
            }
            return t;
        }
        public static List<HtmlElement> GetElementsByTagNameClass(this HtmlElement self, String Class, String Tag)
        {
            List<HtmlElement> t = new List<HtmlElement>();
            foreach (HtmlElement hel in self.GetElementsByTagName(Tag))
            {
                if (hel.GetAttribute("className").Split(' ', '\t').Contains(Class))
                    t.Add(hel);
            }
            return t;
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
        #endregion


        #region DEMO

        #endregion
    }
}
