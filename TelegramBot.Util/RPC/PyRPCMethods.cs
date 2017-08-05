//using IpcPythonCS.Engine.CSharp.Communication;
//using IpcPythonCS.Engine.CSharp.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.RPC
{
    public class PyRPCMethods
    {
        public PyRPCMethods()
        {

        }

        public string ExecutePythonCommand(string command, string arg)
        {
            string requestString;
            
            requestString = String.Format("{{ \"command\": \"{0}\", \"arg\": \"{1}\" }}", command, arg);

            return GetPostRequest("http://localhost:5050/main.php", requestString);
        }

        protected static string GetPostRequest(string url, string requestString)
        {
            byte[] data = Encoding.UTF8.GetBytes(requestString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT x.y; rv:10.0) Gecko/20100101 Firefox/10.0";
            request.Proxy = null;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            string responseContent;

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }

            return responseContent;
        }
    }
}
