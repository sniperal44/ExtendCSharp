using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class OnlineServices:IService
    {
        public OnlineServices() { }
        
        public byte[] GetFileViaHttp(string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    return client.DownloadData(url);
                }
               catch (Exception e)
                {
                    return null;

                }
            }
        }
   
        public async Task<byte[]> GetFileViaHttpAsync(string url, DownloadProgressChangedEventHandler DownloadProgressChanged, DownloadDataCompletedEventHandler DownloadDataCompleted)
        {    
            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += DownloadDataCompleted;
                client.DownloadProgressChanged += DownloadProgressChanged;
                return await client.DownloadDataTaskAsync(new Uri(url));
            }
            
        }


        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public  String GetStringViaHttp(string url)
        {

            throw new NotImplementedException();
            /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return data;
            }

            return null;*/
        }


    }
}
