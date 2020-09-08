using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExtendCSharp;

[assembly: InternalsVisibleToAttribute("CancellationTokenExtend")]
namespace ExtendCSharp.ExtendedClass
{

    public class CancellationTokenSourcePlus :CancellationTokenSource
    {
        internal static Dictionary<CancellationToken, String> messages = new Dictionary<CancellationToken, string>();
        public void Cancel(String Message)
        {
            messages[Token] = Message;
            base.Cancel();
        }
    }

    public static class CancellationTokenExtend
    {
        public static String GetMessage(this CancellationToken ct)
        {
            if(CancellationTokenSourcePlus .messages.ContainsKey(ct))
            {
                return CancellationTokenSourcePlus .messages.RemoveAndGet(ct);
            }
            return null;
        }
    }

}
