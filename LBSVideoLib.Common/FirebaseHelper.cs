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
        private const string URL = "https://lbf-video-content.firebaseio.com/lbf-video-content/registrations-data/{0}/.json";


        public static void PostData(string requestJson, string schoolcode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(URL, schoolcode));
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("API-KEY", "");
            request.ContentLength = requestJson.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(requestJson);
            requestWriter.Close();
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }

        }
    }
}
