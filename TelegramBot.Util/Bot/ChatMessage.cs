using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot
{
    public class ChatMessage
    {
        public long UpdateID;
        public MessageArticle Message;

        /// <summary>
        /// JSON 파싱시 발생하는 오류 없이 안전하게 값을 받아오는 매서드
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetSafeKeyValue(Dictionary<string, object> dict, string key, string defaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key].ToString();
            }

            return defaultValue;
        }

        /// <summary>
        /// JSON 파싱시 발생하는 오류 없이 안전하게 값을 받아오는 매서드
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static long GetSafeKeyValue(Dictionary<string, object> dict, string key, long defaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return Convert.ToInt64(dict[key]);
            }

            return defaultValue;
        }

        public static ChatMessage FromJsonString(string json)
        {
            /*
             {"update_id":820677280,"message":{"message_id":196,"from":{"id":208394200,"first_name":"FirstName","last_name":"LastName","language_code":"en-US"},"chat":{"id":208394200,"first_name":"FirstName","last_name":"LastName","type":"private"},"date":1499341978,"text":"\ubd07\ud14c\uc2a4\ud2b8"}}
            */

            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Dictionary<string, object> msg = JsonConvert.DeserializeObject<Dictionary<string, object>>(values["message"].ToString());
            Dictionary<string, object> from = JsonConvert.DeserializeObject<Dictionary<string, object>>(msg["from"].ToString());
            Dictionary<string, object> chat = JsonConvert.DeserializeObject<Dictionary<string, object>>(msg["chat"].ToString());

            ChatMessage msgEnv = new ChatMessage();
            msgEnv.UpdateID = GetSafeKeyValue(values, "update_id", 0);

            msgEnv.Message = new MessageArticle();
            msgEnv.Message.MessageID = GetSafeKeyValue(msg, "message_id", 0);

            msgEnv.Message.From = new MessageArticle._SenderInfo();
            msgEnv.Message.From.ID = GetSafeKeyValue(from, "id", 0);
            msgEnv.Message.From.FirstName = GetSafeKeyValue(from, "first_name", "");
            msgEnv.Message.From.LastName = GetSafeKeyValue(from, "last_name", "");
            msgEnv.Message.From.LanguageCode = GetSafeKeyValue(from, "language_code", "");

            msgEnv.Message.Chat = new MessageArticle._ChatInfo();
            msgEnv.Message.Chat.ID = GetSafeKeyValue(chat, "id", 0);
            msgEnv.Message.Chat.Type = GetSafeKeyValue(chat, "type", "");

            // parse private chat / group chat info
            if (msgEnv.Message.Chat.Type.Equals("private", StringComparison.InvariantCultureIgnoreCase))
            {
                msgEnv.Message.Chat.FirstName = GetSafeKeyValue(chat, "first_name", "");
                msgEnv.Message.Chat.LastName = GetSafeKeyValue(chat, "last_name", "");
            }
            else if (msgEnv.Message.Chat.Type.Equals("group", StringComparison.InvariantCultureIgnoreCase))
            {
                msgEnv.Message.Chat.GroupChatTitle = GetSafeKeyValue(chat, "title", "");
                msgEnv.Message.Chat.GroupChatIsAllAdmin = (GetSafeKeyValue(chat, "all_members_are_administrators", "")).Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }

            msgEnv.Message.Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            msgEnv.Message.Date = msgEnv.Message.Date.AddSeconds(GetSafeKeyValue(msg, "date", 0)).ToLocalTime();
            msgEnv.Message.Text = GetSafeKeyValue(msg, "text", "");

            return msgEnv;
        }
    }
}
