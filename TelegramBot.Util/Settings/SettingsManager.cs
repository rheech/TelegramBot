using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace TelegramBot.Util.Settings
{
    public class SettingsManager
    {
        private string _botName;

        public SettingsManager(string botName)
        {
            _botName = botName;
        }

        private string GetSetting(string section, string defaultValue = "")
        {
            try
            {
                using (RegistryKey basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (RegistryKey subkey = basekey.OpenSubKey(String.Format("SOFTWARE\\VB and VBA Program Settings\\{0}\\{1}", "TelegramBot", _botName)))
                    {
                        return subkey.GetValue(section).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        private void SaveSetting(string section, string value)
        {
            using (RegistryKey basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey subkey = basekey.CreateSubKey(String.Format("SOFTWARE\\VB and VBA Program Settings\\{0}\\{1}", "TelegramBot", _botName)))
                {
                    subkey.SetValue(section, value, RegistryValueKind.String);
                }
            }
            
            //Interaction.SaveSetting("TelegramBot", _botName, section, value);
        }

        public string BotToken
        {
            get
            {
                return GetSetting("BotToken");
            }
            set
            {
                SaveSetting("BotToken", value);
            }
        }

        public bool BotStopped
        {
            get
            {
                try
                {
                    return bool.Parse(GetSetting("BotStopped"));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                SaveSetting("BotStopped", value.ToString());
            }
        }

        public string PythonExePath
        {
            get
            {
                return GetSetting("PythonExePath");
            }
            set
            {
                SaveSetting("PythonExePath", value);
            }
        }

        public string IPCPythonModulePath
        {
            get
            {
                return GetSetting("IPCPythonModulePath");
            }
            set
            {
                SaveSetting("IPCPythonModulePath", value);
            }
        }

        public long DefaultRcpt
        {
            get
            {
                return Int64.Parse(GetSetting("DefaultRcpt"));
            }
            set
            {
                SaveSetting("DefaultRcpt", value.ToString());
            }
        }

        public long RegisteredRoom1
        {
            get
            {
                return Int64.Parse(GetSetting("RegisteredRoom1"));
            }
            set
            {
                SaveSetting("RegisteredRoom1", value.ToString());
            }
        }

        public long RegisteredRoom2
        {
            get
            {
                return Int64.Parse(GetSetting("RegisteredRoom2"));
            }
            set
            {
                SaveSetting("RegisteredRoom2", value.ToString());
            }
        }


        public string OracleURL
        {
            get
            {
                return GetSetting("OracleURL");
            }
            set
            {
                SaveSetting("OracleURL", value.ToString());
            }
        }

        public int OraclePort
        {
            get
            {
                return Int32.Parse(GetSetting("OraclePort"));
            }
            set
            {
                SaveSetting("OraclePort", value.ToString());
            }
        }

        public string OracleDBName
        {
            get
            {
                return GetSetting("OracleDBName");
            }
            set
            {
                SaveSetting("OracleDBName", value);
            }
        }

        public string OracleUserName
        {
            get
            {
                return GetSetting("OracleUserName");
            }
            set
            {
                SaveSetting("OracleUserName", value);
            }
        }

        public string OracleUserPassword
        {
            get
            {
                return GetSetting("OracleUserPassword");
            }
            set
            {
                SaveSetting("OracleUserPassword", value);
            }
        }

        public string NaverAPIClientID
        {
            get
            {
                return GetSetting("NaverAPIClientID");
            }
            set
            {
                SaveSetting("NaverAPIClientID", value);
            }
        }

        public string NaverAPIClientSecret
        {
            get
            {
                return GetSetting("NaverAPIClientSecret");
            }
            set
            {
                SaveSetting("NaverAPIClientSecret", value);
            }
        }

        public string LastBlocker
        {
            get
            {
                return GetSetting("LastBlocker");
            }
            set
            {
                SaveSetting("LastBlocker", value);
            }
        }

        public DateTime LastQueriedDate
        {
            get
            {
                long timeTick;
                
                try
                {
                    timeTick = Int64.Parse(GetSetting("LastQueriedDate"));
                }
                catch (Exception ex)
                {
                    timeTick = 0;
                }

                return new DateTime(timeTick);
            }
            set
            {
                SaveSetting("LastQueriedDate", value.Ticks.ToString());
            }
        }

        public DateTime LastWarnedDate
        {
            get
            {
                long timeTick;

                try
                {
                    timeTick = Int64.Parse(GetSetting("LastWarnedDate"));
                }
                catch (Exception ex)
                {
                    timeTick = 0;
                }

                return new DateTime(timeTick);
            }
            set
            {
                SaveSetting("LastWarnedDate", value.Ticks.ToString());
            }
        }

        public int MessageDelay
        {
            get
            {
                int msgDelay;

                try
                {
                    msgDelay = Int32.Parse(GetSetting("MessageDelay"));
                }
                catch (Exception ex)
                {
                    msgDelay = 5;
                }

                return msgDelay;
            }
            set
            {
                SaveSetting("MessageDelay", value.ToString());
            }
        }

        public int MessageDelayDefault
        {
            get
            {
                int msgDelay;

                try
                {
                    msgDelay = Int32.Parse(GetSetting("MessageDelayDefault"));
                }
                catch (Exception ex)
                {
                    msgDelay = 5;
                }

                return msgDelay;
            }
            set
            {
                SaveSetting("MessageDelayDefault", value.ToString());
            }
        }
    }
}
