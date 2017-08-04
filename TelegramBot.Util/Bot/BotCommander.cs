﻿using IpcPythonCS.Engine.CSharp;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Util.RPC;
using TelegramBot.Util.Settings;

namespace TelegramBot.Util.Bot
{
    public class BotCommander
    {
        PythonExecutor executor;
        SettingsManager _settings;

        public BotCommander(SettingsManager settings)
        {
            _settings = settings;
            executor = new PythonExecutor(_settings.PythonExePath, _settings.IPCPythonModulePath);
            executor.RunScript("main.py");
        }

        private bool IsCommand(string requestedMessage)
        {
            return requestedMessage.Length > 0 && requestedMessage.Substring(0, 1) == "/";
        }

        private string[] ParseCommand(string requestedMessage)
        {
            string[] args = null;

            if (IsCommand(requestedMessage))
            {
                args = requestedMessage.Split(new string[] { " " }, 2, StringSplitOptions.None);
                args[0] = args[0].Substring(1).ToLowerInvariant();
                args[0] = args[0].Replace("@cheongbot", ""); // remove unnecessary bot's name in command

                return args;
            }

            throw new Exception("Error occurred while parsing command");
        }

        public bool ProcessCommand(string requestedMessage, out string msgReturn)
        {
            string[] args;

            msgReturn = "";

            // Process only if the message begins with "/" (command)
            if (!IsCommand(requestedMessage))
            {
                return false;
            }

            args = ParseCommand(requestedMessage);

            switch (args[0])
            {
                case "말해":
                    msgReturn = GetSayItCommand(args);
                    break;
                case "시간":
                    msgReturn = GetCurrentTime(args);
                    break;
                case "version":
                    msgReturn = GetCurrentVersion(args);
                    break;
                case "calc":
                    msgReturn = GetCalculator();
                    break;
                case "날씨":
                    msgReturn = GetWeatherByLocation(args);
                    break;
                case "?":
                case "help":
                    msgReturn = GetHelp(args);
                    break;
                default:
                    msgReturn = "지원하지 않는 명령어 입니다. /? 또는 /help 를 입력하여 사용법을 확인하세요.";
                    break;
            }

            return true;
        }

        private PipeClient ConnectPipe()
        {
            PipeClient pipeClient = new PipeClient();
            pipeClient.Connect("pyrpcmethods");

            return pipeClient;
        }

        private string GetCalculator()
        {
            PipeClient pipeClient = ConnectPipe();
            PyRPCMethods methods = new PyRPCMethods(pipeClient);

            int rtn = methods.Addition(1, 2);

            pipeClient.Close();

            return rtn.ToString();
        }

        private string GetSuwonWeather()
        {
            PipeClient pipeClient = ConnectPipe();
            PyRPCMethods methods = new PyRPCMethods(pipeClient);

            string rtn = methods.GetSuwonWeather();
            pipeClient.Close();

            return rtn.ToString();
        }

        private string GetWeatherByLocation(string[] args)
        {
            using (PipeClient pipeClient = ConnectPipe())
            {
                PyRPCMethods methods = new PyRPCMethods(pipeClient);

                if (args.Length > 1)
                {
                    string rtn = methods.GetWeatherByLocation(args[1]);

                    return rtn.ToString();
                }

                return "날씨 위치를 입력하세요.";
            }
        }

        private string GetSayItCommand(string[] args)
        {
            if (args.Length > 1)
            {
                return args[1];
            }

            return "오류: 입력 값 없음";
        }

        private string GetCurrentTime(string[] args)
        {
            DateTime dt;
            dt = DateTime.Now;

            return String.Format("현재 시간은 {0}시 {1}분 {2}초입니다.", dt.Hour, dt.Minute, dt.Second);
        }

        private string GetCurrentVersion(string[] args)
        {
            return "0.0.2 Beta";
        }

        private string GetHelp(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("사용법\r\n");
            sb.AppendFormat("/말해 <문장>: 입력한 말을 반복\r\n");
            sb.AppendFormat("/시간: 현재 시간 출력\r\n");
            sb.AppendFormat("/version: 현재 버전 출력");

            return sb.ToString();
        }
    }
}