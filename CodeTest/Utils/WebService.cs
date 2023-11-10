using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Quote.Responses;

namespace PruebaIngreso.Utils
{
    public class WebService
    {
        private static WebService singleton_instance;
        private WebService() { }
        public static WebService getInstance()
        {
            if (singleton_instance == null)
                singleton_instance = new WebService();
            return singleton_instance;
        }
        public RootMarginResponse invoke(string URL,string code) {
            RootMarginResponse root = new RootMarginResponse();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + code);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 5000;//Si el servicio no responde en este tiempo,se cancela la espera

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        root.code = (int)httpResponse.StatusCode;
                        root.messagge = "OK";
                        root.data = JsonConvert.DeserializeObject<MarginResponse>(streamReader.ReadToEnd());
                    }
                }
                else
                {
                    root.code = (int)httpResponse.StatusCode;
                    root.messagge = "ERROR";
                    MarginResponse margin = new MarginResponse();
                    margin.margin = 0.0M;
                }
            }
            catch (Exception ex)
            {
                root.code = 500;
                root.messagge = "ERROR";
                MarginResponse margin = new MarginResponse();
                margin.margin = 0.0M;
            }
            return root;
        }
    }
}