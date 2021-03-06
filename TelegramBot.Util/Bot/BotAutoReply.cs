﻿using MySql.Data.MySqlClient;
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
            int count = 0;

            _cmd.CommandText = String.Format("SELECT * FROM `autoreply`;");

            StringBuilder sb = new StringBuilder();

            //sb.Append("키워드 목록:\r\n");

            using (MySqlDataReader rdr = _cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    sb.AppendFormat("{0}\r\n", rdr["Message"]);
                    count++;
                }

                if (sb.Length > 2)
                {
                    sb.Remove(sb.Length - 2, 2);
                }
            }

            return String.Format("키워드 목록 (총 {0}개):\r\n{1}", count, sb.ToString());
        }

        private string GetSafeQuery(string sql)
        {
            return sql.Replace("'", "").Replace("`", "");
        }

        public void RegisterMessage(string author, string message, string reply)
        {
            // INSERT INTO table (id, name, age) VALUES(1, "A", 19) ON DUPLICATE KEY UPDATE name="A", age=19

            message = GetSafeQuery(message);

            // remove trim() method & prevent sql injection on reply
            _cmd.CommandText = String.Format("INSERT INTO `autoreply` (Author, Message, Reply, RegTime) VALUES ('{0}', '{1}', '{2}', NOW()) ON DUPLICATE KEY UPDATE Author = '{0}', Message = '{1}', Reply = '{2}';",
                        author, message.Trim(), System.Uri.EscapeDataString(reply.Replace("'", "&squot;")));

            _cmd.ExecuteNonQuery();
        }

        public void DeleteMessage(string message)
        {
            // INSERT INTO table (id, name, age) VALUES(1, "A", 19) ON DUPLICATE KEY UPDATE name="A", age=19
            message = GetSafeQuery(message);

            // remove trim() method & prevent sql injection on reply
            _cmd.CommandText = String.Format("DELETE FROM `autoreply` WHERE Message = '{0}'", message.Trim());

            _cmd.ExecuteNonQuery();
        }

        public string FindMessage(string message)
        {
            string rtn = "";

            message = GetSafeQuery(message);

            _cmd.CommandText = String.Format("SELECT * FROM `autoreply` WHERE `Message` = '{0}';", message.Trim());

            using (MySqlDataReader rdr = _cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    //rtn = String.Format("[{0}] {1}", rdr["Author"], rdr["Reply"]);
                    try
                    {
                        //rtn = String.Format("[{0}] {1}", rdr["Author"], System.Uri.UnescapeDataString(rdr["Reply"].ToString()).Replace("&squot;", "'"));
                        rtn = String.Format("{1}", rdr["Author"], System.Uri.UnescapeDataString(rdr["Reply"].ToString()).Replace("&squot;", "'"));
                    }
                    catch (Exception)
                    {
                        //rtn = String.Format("[{0}] {1}", rdr["Author"], rdr["Reply"]);
                        rtn = String.Format("{1}", rdr["Author"], rdr["Reply"].ToString());
                    }
                }
            }

            return rtn;
        }
    }
}
