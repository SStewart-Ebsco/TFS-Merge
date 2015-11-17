using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;

namespace ER_BestPickReports_Dev.Helpers
{
    public class HttpRequestHelper
    {
        public static string GetClientIp(HttpRequest request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static string Get(string url)
        {
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static T GetJson<T>(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof (T));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                T jsonResponse = (T) objResponse;
                return jsonResponse;
            }
        }
    }
}