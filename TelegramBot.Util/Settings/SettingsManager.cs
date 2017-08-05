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
