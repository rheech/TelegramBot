using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot
{
    public class BotAutoReply : DB.DatabaseIO
    {
        public BotAutoReply(string databaseURL, int port, string databaseName, string ID, string password)
            : base(databaseURL, port, databaseName, ID, password)
        {
            
        }

        public string RetrieveAllKeywords()
        {
            string rtn = "";

            _cmd.CommandText = String.Format("SELECT * FROM `autoreply`;");

            StringBuilder sb = new StringBuilder();

            using (MySqlDataReader rdr = _cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    sb.AppendFormat("{0}\r\n", rdr["Message"]);
                }

                if (sb.Length > 2)
                {
                    sb.Remove(sb.Length - 2, 2);
                }
            }

            return sb.ToString();
        }

        public void RegisterMessage(string author, string message, string reply)
        {
            // INSERT INTO table (id, name, age) VALUES(1, "A", 19) ON DUPLICATE KEY UPDATE name="A", age=19

            _cmd.CommandText = String.Format("INSERT INTO `autoreply` (Author, Message, Reply, RegTime) VALUES ('{0}', '{1}', '{2}', NOW()) ON DUPLICATE KEY UPDATE Author = '{0}', Message = '{1}', Reply = '{2}';",
                        author, message.Trim(), reply.Trim());

            _cmd.ExecuteNonQuery();
        }

        public string FindMessage(string message)
        {
            string rtn = "";

            _cmd.CommandText = String.Format("SELECT * FROM `autoreply` WHERE `Message` = '{0}';", message.Trim());

            using (MySqlDataReader rdr = _cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    rtn = String.Format("[{0}] {1}", rdr["Author"], rdr["Reply"]);
                }
            }

            return rtn;
        }
    }
}
