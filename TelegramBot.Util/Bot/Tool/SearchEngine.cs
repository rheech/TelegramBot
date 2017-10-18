using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot.Tool
{
    public class SearchEngine
    {
        public struct SearchResult
        {
            public SearchEntry[] items;
        }

        public struct SearchEntry
        {
            public string title;
            public string link;
            public string description;
            public string bloggername;
            public string bloggerlink;
            public string postdate;
        }

        private string _clientID;
        private string _clientSecret;

        public SearchEngine(string clientID, string clientSecret)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;
        }

        public SearchResult SearchNaverBlog(string query)
        {
            string url = "https://openapi.naver.com/v1/search/blog?query=" + query; // 결과가 JSON 포맷
            // string url = "https://openapi.naver.com/v1/search/blog.xml?query=" + query;  // 결과가 XML 포맷
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", _clientID); // 클라이언트 아이디
            request.Headers.Add("X-Naver-Client-Secret", _clientSecret);       // 클라이언트 시크릿
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string status = response.StatusCode.ToString();
            if (status == "OK")
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadToEnd();

                SearchResult result = JsonConvert.DeserializeObject<SearchResult>(text);

                return result;
            }
            else
            {
                throw new Exception(String.Format("Error: {0}", status));
            }
        }
    }
}
