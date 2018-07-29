using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LBFVideoLib.Common
{
    public static class FirebaseHelper
    {
        //private const string URL = "https://lbf-video-content.firebaseio.com/registrations-data/{0}/.json";
        private const string BaseURL = "https://lbf-video-content.firebaseio.com/{0}/.json";


        public static void PostData(string requestJson, string url)
        {
            string requestUrl = string.Format(BaseURL, url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("API-KEY", "");
            request.ContentLength = requestJson.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(requestJson);
            requestWriter.Close();

            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            Console.Out.WriteLine(response);
            responseReader.Close();
        }
    }
}
