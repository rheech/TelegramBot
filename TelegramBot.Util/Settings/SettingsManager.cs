using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using IniParser;
using IniParser.Model;

namespace TelegramBot.Util.Settings
{
    public class SettingsManager
    {
        private string _botName;
        private string[] _keys = new string[] { "BotToken", "PythonExePath", "IpcPythonModulePath", "DefaultRcpt" };
        private Dictionary<string, string> _dictSettings;

        public SettingsManager(string botName)
        {
            _botName = botName;

            LoadConfig();
        }

        private void LoadConfig()
        {
            string[] values;

            DirectoryInfo configPath = new DirectoryInfo(Environment.GetEnvironmentVariable("BOT_CONFIG_HOME"));

            values = System.IO.File.ReadAllLines(String.Format("{0}{1}.ini", configPath.FullName, _botName));

            _dictSettings = new Dictionary<string, string>();

            for (int i = 0; i < values.Length; i++)
            {
                _dictSettings.Add(_keys[i], values[i]);
            }
        }

        private string GetSetting(string section, string defaultValue = "")
        {
            try
            {
                return _dictSettings[section];
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        private void SaveSetting(string section, string value)
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(String.Format("Software\\VB and VBA Program Settings\\{0}\\{1}", "TelegramBot", _botName));
                
            key.SetValue(section, value, RegistryValueKind.String);
            

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
