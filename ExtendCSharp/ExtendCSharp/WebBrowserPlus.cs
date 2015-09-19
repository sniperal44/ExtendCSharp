using CsQuery;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp
{
    public class WebBrowserPlus : WebBrowser,IDisposable
    {
        static bool HandlerLibAlreadyAttached=false;



        static WebBrowserPlus()
        {
            if (!WebBrowserPlus.HandlerLibAlreadyAttached)
            {
                AppDomain.CurrentDomain.AssemblyResolve += (s, args) =>
                {
                    string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
                    dllName = dllName.Replace(".", "_");
                    if (dllName.EndsWith("_resources"))
                        return null;
                    System.Resources.ResourceManager rm = new System.Resources.ResourceManager(typeof(WebBrowserPlus).Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                    byte[] bytes = (byte[])rm.GetObject(dllName);
                    return System.Reflection.Assembly.Load(bytes);
                };
                WebBrowserPlus.HandlerLibAlreadyAttached = true;
            }
        }

        public WebBrowserPlus() : base()
        {

        }



        public new  HtmlDocument  Document
         {
            get
            {
                if (InvokeRequired)
                    return (HtmlDocument)Invoke((Func<HtmlDocument>)delegate { return base.Document; });
                else
                    return base.Document;
            }
        }
        public new Uri Url
        {
            get
            {      
                if (InvokeRequired)
                    return (Uri)Invoke((Func<Uri>)delegate { return base.Url; });
                else
                    return base.Url;
            }
            set
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { base.Url=value; });
                else
                    base.Url = value;
            }


        }
        public new String DocumentText
        {
            get
            {
                if (InvokeRequired)
                    return (String)Invoke((Func<String>)delegate { return base.DocumentText; });
                else
                    return base.DocumentText;
            }
            set
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { base.DocumentText = value; });
                else
                    base.DocumentText = value;
            }
        }
        public String Html
        {
            get
            {
                return DocumentText;
            }
            set
            {
                DocumentText = value;
            }
        }
        public CQ HtmlCQ
        {
            get
            {
                return DocumentText;
            }
        }


        public delegate void WebBrowserPlusDocumentCompleteEventHandler();
        public void NavigateBegin(String Url, WebBrowserPlusDocumentCompleteEventHandler Handler)
        { 
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(Uri Url, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(String Url, bool NewWindow, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,NewWindow, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, NewWindow);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(Uri Url, bool NewWindow, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,NewWindow, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, NewWindow);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(String Url, string TargetFrame, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,TargetFrame, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(Uri Url, string TargetFrame, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,TargetFrame, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(String Url, string TargetFrame, byte[] PostData, String AdditionalHeaders, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,TargetFrame,PostData,AdditionalHeaders, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }

        }
        public void NavigateBegin(Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url,TargetFrame,PostData,AdditionalHeaders, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(String Url, string TargetFrame, String PostData, String AdditionalHeaders, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url, TargetFrame, PostData, AdditionalHeaders, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }
        public void NavigateBegin(Uri Url, string TargetFrame, String PostData, String AdditionalHeaders, WebBrowserPlusDocumentCompleteEventHandler Handler)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NavigateBegin(Url, TargetFrame, PostData, AdditionalHeaders, Handler); });
            }
            else
            {
                if (Handler != null)
                {
                    Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset);
                    WebBrowserDocumentCompletedEventHandler h = null;
                    h = (s, e) =>
                    {
                        Handler();
                        DocumentCompleted -= h;
                    };
                    DocumentCompleted += h;
                }
            }
        }




        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(String s)
        {
            base.Navigate(s);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(Uri Url)
        {
            base.Navigate(Url);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(String Url, bool NewWindow)
        {
            base.Navigate(Url, NewWindow);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(Uri Url, bool NewWindow)
        {
            base.Navigate(Url, NewWindow);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(String Url, string TargetFrame)
        {
            base.Navigate(Url, TargetFrame);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(Uri Url, string TargetFrame)
        {
            base.Navigate(Url, TargetFrame);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(String Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            base.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Navigate(Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            base.Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Navigate( String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            System.Text.Encoding a = System.Text.Encoding.UTF8;
            byte[] byte1 = a.GetBytes(PostData);
            base.Navigate(Url, TargetFrame, byte1, AdditionalHeaders);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Navigate( Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            System.Text.Encoding a = System.Text.Encoding.UTF8;
            byte[] byte1 = a.GetBytes(PostData);
            base.Navigate(Url, TargetFrame, byte1, AdditionalHeaders);
        }



        public void NavigateInvoke(String Url)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url); });
            else
                Navigate(Url);
        }
        public void NavigateInvoke(Uri Url)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url); });
            else
                Navigate(Url);
        }
        public void NavigateInvoke(String Url, bool NewWindow)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, NewWindow); });
            else
                Navigate(Url, NewWindow);
        }
        public void NavigateInvoke(Uri Url, bool NewWindow)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, NewWindow); });
            else
                Navigate(Url, NewWindow);
        }
        public void NavigateInvoke(String Url, string TargetFrame)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame); });
            else
                Navigate(Url, TargetFrame);
        }
        public void NavigateInvoke(Uri Url, string TargetFrame)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame); });
            else
                Navigate(Url, TargetFrame);
        }
        public void NavigateInvoke(String Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                Navigate(Url, TargetFrame, PostData, AdditionalHeaders);

        }
        public void NavigateInvoke(Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        public void NavigateInvoke(String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }
        public void NavigateInvoke(Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders); });
            else
                Navigate(Url, TargetFrame, PostData, AdditionalHeaders);
        }


        public void NavigateAndWait(String Url)
        {
            NavigateInvoke(Url);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(Uri Url)
        {
            NavigateInvoke(Url);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(String Url, bool NewWindow)
        {
            NavigateInvoke(Url, NewWindow);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(Uri Url, bool NewWindow)
        {
            NavigateInvoke(Url, NewWindow);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(String Url, string TargetFrame)
        {
            NavigateInvoke(Url, TargetFrame);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(Uri Url, string TargetFrame)
        {
            NavigateInvoke(Url, TargetFrame);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(String Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            WaitCompleteInvoke();

        }
        public void NavigateAndWait(Uri Url, string TargetFrame, byte[] PostData, String AdditionalHeaders)
        {
            NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(String Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            WaitCompleteInvoke();
        }
        public void NavigateAndWait(Uri Url, string TargetFrame, String PostData, String AdditionalHeaders)
        {
            NavigateInvoke(Url, TargetFrame, PostData, AdditionalHeaders);
            WaitCompleteInvoke();
        }

        public void WaitCompleteInvoke()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { WaitCompleteInvoke(); });
            }
            else
            {
                try
                {
                    while (ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
                }
                catch(Exception ex) { }
            }
        }


   


        public new void Dispose()
        {
            NavigateBegin("about:blank", () =>
            {
                base.Dispose();
            });
        }




        public static void SetupIEVersion(IEVersion v)
        {
            try
            { 
                var IEVAlue = v; // can be: 9999 , 9000, 8888, 8000, 7000
                var targetApplication = Process.GetCurrentProcess().ProcessName + ".exe";
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl", true);
                RegistryKey f = rk.CreateSubKey("FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, IEVAlue, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_AJAX_CONNECTIONEVENTS", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_DOMSTORAGE ", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_GPU_RENDERING ", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_DISABLE_LEGACY_COMPRESSION", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_LOCALMACHINE_LOCKDOWN", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_BLOCK_LMZ_OBJECT", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_BLOCK_LMZ_SCRIPT", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_SCRIPTURL_MITIGATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_SPELLCHECKING", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_STATUS_BAR_THROTTLING", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_TABBED_BROWSING", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_VALIDATE_NAVIGATE_URL", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_WEBOC_DOCUMENT_ZOOM", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_WEBOC_POPUPMANAGEMENT", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_WEBOC_MOVESIZECHILD", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_ADDON_MANAGEMENT", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_WEBSOCKET", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_WINDOW_RESTRICTIONS ", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 0, RegistryValueKind.DWord);
                f = rk.CreateSubKey("FEATURE_XMLHTTP", RegistryKeyPermissionCheck.ReadWriteSubTree);
                f.SetValue(targetApplication, 1, RegistryValueKind.DWord);




                rk.Close();
                rk.Dispose();
                f.Close();
                f.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show("NOTE: you need to run this under no UAC");
            }
        }
    }

    public enum IEVersion
    {
        IE11Edge = 11001,
        IE11 = 11000,
        IE10ForceStandard = 10001,
        IE10 = 10000,
        IE9ForceStandard = 9999,
        IE9 = 9000,
        IE8ForceStandard = 8888,
        IE8 = 8000,
        IE7 = 7000

    };
}
