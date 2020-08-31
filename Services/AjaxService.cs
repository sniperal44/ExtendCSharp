using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExtendCSharp.Services
{
    /// <summary>
    /// permette di semplificare chiamate "ajax" a API rest ( GET o POST )
    /// </summary>
    public class AjaxService: IService
    {
        static HttpClient client = new HttpClient();
        public async Task<string> POST(String Url, AjaxPayload payload=null)
        {
            string UrlEncoded_string;
            if (payload == null)
                UrlEncoded_string = "";
            else
                UrlEncoded_string = payload.GetPayload();


            HttpContent c = new StringContent(UrlEncoded_string, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Url),
                Content = c 
            };
            
            HttpResponseMessage result = await client.SendAsync(request);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> GET(String Url, AjaxPayload payload)
        {

            string UrlEncoded_string;
            if (payload == null)
                UrlEncoded_string = "";
            else
                UrlEncoded_string = "?"+payload.GetPayload();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Url+UrlEncoded_string)
            };

            HttpResponseMessage result = await client.SendAsync(request);
            return await result.Content.ReadAsStringAsync();
        }
    }



    /*
     * example:

        AjaxPayload root = new AjaxPayload();
        root.AddNode("Get", "1");
        root.AddNode("Question", "aa");
        AjaxPayload s1 = root.AddNode("pippo").SubPayload;
        s1.AddNode("1", "ciao");
        s1.AddNode("2", "mondo");
        AjaxPayload s2 = s1.AddNode("3").SubPayload;
        s2.AddNode("ass1", "test1");
        s2.AddNode("ass2", "test2");
    */
    public class AjaxPayload
    {
        List<AjaxPayloadNode> nodes = new List<AjaxPayloadNode>();

        public AjaxPayloadNode AddNode(string Key,string Value)
        {
            AjaxPayloadNode t = new AjaxPayloadNode(Key, Value);
            nodes.Add(t);
            return t;
        }
        public AjaxPayloadNode AddNode(string Key)
        {
            AjaxPayloadNode t = new AjaxPayloadNode(Key);
            nodes.Add(t);
            return t;
        }


        public void RemoveNode(AjaxPayloadNode node)
        {
            nodes.Remove(node);
        }
        public void RemoveNode(String Key)
        {
            AjaxPayloadNode t = null;
            foreach(AjaxPayloadNode n in nodes)
            {
                if(n.Key== Key)
                {
                    t = n;
                    break;
                }
            }
            if (t != null)
                RemoveNode(t);
        }



        public string GetPayload(string ArrayName=null)
        {
            string s = "";
            foreach (AjaxPayloadNode n in nodes)
            {
                s += n.GetPayload(ArrayName) + "&";
            }
            return s.RemoveRight("&");
        }

    }
    public class AjaxPayloadNode
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public AjaxPayload SubPayload { get; private set; } = null;

        public bool isArray
        {
            get
            {
                return SubPayload != null;
            }
        }
        public AjaxPayloadNode(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public AjaxPayloadNode(string key)
        {
            Key = key;
            SubPayload = new AjaxPayload();
        }



        public string GetPayload(string ArrayName=null)
        {
            string s = "";
            if (ArrayName != null)
            {
                if (isArray)
                {
                    s =  SubPayload.GetPayload(ArrayName + "[" + Key + "]");
                }
                else
                {
                    s = ArrayName + "[" + Key + "]" + "=" + HttpUtility.UrlEncode(Value);
                }
            }
            else
            {
                if (isArray)
                {
                    s = SubPayload.GetPayload(Key);
                }
                else
                {
                    s = Key + "=" + HttpUtility.UrlEncode(Value);
                }
            }
            return s;
        }
    }
}
