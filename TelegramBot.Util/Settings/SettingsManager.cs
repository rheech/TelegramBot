using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

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
            return Interaction.GetSetting("TelegramBot", _botName, section, defaultValue);
        }

        private void SaveSetting(string section, string value)
        {
            Interaction.SaveSetting("TelegramBot", _botName, section, value);
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
    }
}
