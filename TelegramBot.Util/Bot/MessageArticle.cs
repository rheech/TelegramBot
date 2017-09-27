using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot
{
    public class MessageArticle
    {
        public long MessageID;
        public _SenderInfo From;
        public _ChatInfo Chat;
        public DateTime Date;
        public string Text;

        public struct _SenderInfo
        {
            public long ID;
            public string FirstName;
            public string LastName;
            public string LanguageCode;

            public override string ToString()
            {
                return String.Format("{0} {1}", FirstName, LastName).Trim();
            }
        }

        public struct _ChatInfo
        {
            public long ID;
            public string FirstName;
            public string LastName;
            public string Type;
            public string GroupChatTitle;
            public bool GroupChatIsAllAdmin;
        }

        /*
         {"update_id":820677280,"message":{"message_id":196,"from":{"id":208394200,"first_name":"FirstName","last_name":"LastName","language_code":"en-US"},"chat":{"id":208394200,"first_name":"FirstName","last_name":"LastName","type":"private"},"date":1499341978,"text":"\ubd07\ud14c\uc2a4\ud2b8"}}
        */
    }
}
