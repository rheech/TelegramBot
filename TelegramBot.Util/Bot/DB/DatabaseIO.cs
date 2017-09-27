using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Bot.DB
{
    public abstract class DatabaseIO
    {
        protected string DATABASE_NAME = "telegrambot";
        protected string DB_ID, DB_PW, DB_URL;
        protected int DB_PORT;

        protected MySqlConnection _con;
        protected MySqlCommand _cmd;
        private bool _isOpen;

        protected const string SQL_DATE_FORMAT = "%m-%d-%Y %H:%i:%s";

        protected DatabaseIO(string databaseURL, int port, string databaseName, string ID, string password)
        {
            DB_URL = databaseURL;
            DB_PORT = port;
            DATABASE_NAME = databaseName;
            DB_ID = ID;
            DB_PW = password;

            Open();
        }

        ~DatabaseIO()
        {
            if (isOpen)
            {
                Close();
            }
        }

        protected void ExecuteNonQuery(string query)
        {
            ExecuteNonQuery(query, null);
        }

        protected void ExecuteNonQuery(string query, params object[] args)
        {
            if (_cmd != null)
            {
                if (args != null)
                {
                    _cmd.CommandText = String.Format(query, args);
                }
                else
                {
                    _cmd.CommandText = query;
                }

                _cmd.ExecuteNonQuery();
            }
        }

        public MySqlConnection Open(bool eraseDB)
        {
            _con = new MySqlConnection(ConnectionString);
            _con.Open();

            _cmd = new MySqlCommand();
            _cmd.Connection = _con;

            //CreateDatabase(eraseDB);
            _isOpen = true;

            return _con;
        }

        public MySqlConnection Open()
        {
            return Open(false);
        }

        public void Close()
        {
            if (_con != null)
            {
                _con.Close();
                _cmd = null;
            }

            _isOpen = false;
        }

        protected string ConnectionString
        {
            get
            {
                string datasource;
                datasource = String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8;", DB_URL, DB_PORT, DATABASE_NAME, DB_ID, DB_PW);
                return datasource;
            }
        }

        public bool isOpen
        {
            get
            {
                if (_con != null && _con.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
