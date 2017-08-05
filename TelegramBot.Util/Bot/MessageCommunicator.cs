using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot
{
    public class MessageCommunicator
    {
        private const string SEND_MESSAGE = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
        private string _apiKey;

        public MessageCommunicator(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void SendMessage(long chatID, string text)
        {
            // https://api.telegram.org/bot426541811:AAHvsEkn5rEiFgwQic7s6-CCxGP9fDtJQ-U/sendMessage?chat_id=205357200&text=%22Hello!%22

            string msgURL = String.Format(SEND_MESSAGE, _apiKey, chatID, WebUtility.UrlEncode(text));

            GetURLSource(msgURL);
        }

        protected static string GetURLSource(string url)
        {
            string result = "";

            //var client = new WebClient();
            //var text = client.DownloadString(url);

            WebRequest req = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)req;
            
            request.UserAgent = "Mozilla/5.0 (Windows NT x.y; rv:10.0) Gecko/20100101 Firefox/10.0";
            request.Proxy = null;

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
                    string encodingName = response.CharacterSet.ToUpper();
                    int encodingNumber;

                    if (encodingName.StartsWith("MS"))
                    {
                        encodingNumber = Convert.ToInt32(encodingName.Replace("MS", ""));

                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(encodingNumber));
                    }
                    else if (response.CharacterSet == "") // no encoding
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet.ToUpper()));
                    }
                }
                
                result = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                
            }

            return result;
        }
    }
}
