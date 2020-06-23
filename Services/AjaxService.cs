using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    /// <summary>
    /// permette di semplificare chiamate "ajax" a API rest ( GET o POST )
    /// </summary>
    public class AjaxService: IService
    {
        static HttpClient client = new HttpClient();
        public async Task<string> POST(String Url, String UrlEncoded_string)
        {
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

        public async Task<string> GET(String Url, String UrlEncoded_string)
        {
            //HttpContent c = new StringContent(UrlEncoded_string, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Url+"?"+ UrlEncoded_string)
            };

            HttpResponseMessage result = await client.SendAsync(request);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
