﻿using HtmlAgilityPack;
//using IpcPythonCS.Engine.CSharp;
//using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Util.Bot.Tool;
using TelegramBot.Util.RPC;
using TelegramBot.Util.Settings;

namespace TelegramBot.Util.Bot
{
    public class BotCommander
    {
        //PythonExecutor executor;
        SettingsManager _settings;

        public BotCommander(SettingsManager settings)
        {
            _settings = settings;
            /*executor = new PythonExecutor(_settings.PythonExePath, _settings.IPCPythonModulePath);
            executor.RunScript("main.py");*/
        }

        private bool IsCommand(string requestedMessage)
        {
            return requestedMessage.Length > 0 && requestedMessage.Substring(0, 1) == "/";
        }

        private bool IsReturnBotCommand(string requestedMessage)
        {
            return (requestedMessage.Split(' ')[0] == "/돌아와");
        }

        private string[] ParseCommand(string requestedMessage)
        {
            string[] args = null;

            if (IsCommand(requestedMessage))
            {
                args = requestedMessage.Split(new string[] { " " }, 2, StringSplitOptions.None);
                args[0] = args[0].Substring(1).ToLowerInvariant().Trim();
                args[0] = args[0].Replace("@cheongbot", ""); // remove unnecessary bot's name in command

                return args;
            }


            throw new Exception("Error occurred while parsing command");
        }

        public bool ProcessCommand(string author, string requestedMessage, out string msgReturn)
        {
            PyRPCMethods methods = new PyRPCMethods();
            string[] args;

            msgReturn = "";

            int msgDelay = _settings.MessageDelay;

            // 돌아와 command
            if (IsReturnBotCommand(requestedMessage))
            {
                _settings.BotStopped = false;
                _settings.MessageDelay = _settings.MessageDelayDefault;

                msgReturn = ReturnBot();

                return true;
            }

            if ((DateTime.Now - _settings.LastQueriedDate > TimeSpan.FromSeconds(msgDelay)))
            {
                if (_settings.BotStopped)
                {
                    _settings.BotStopped = false;
                    _settings.MessageDelay = _settings.MessageDelayDefault;
                }

                // Process only if the message begins with "/" (command)
                if (!IsCommand(requestedMessage))
                {
                    return false;
                }

                _settings.LastQueriedDate = DateTime.Now;

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
                    case "퇴근시간":
                        msgReturn = GetOffWorkTime(args);
                        break;
                    case "기능개선":
                        break;
                    case "학습":
                        msgReturn = LearnWord(author, requestedMessage.Trim());
                        break;
                    case "키워드":
                        msgReturn = RetrieveAllKeywords();
                        break;
                    case "키워드삭제":
                        msgReturn = DeleteKeyword(args);
                        break;
                    case "검색":
                        msgReturn = SearchNaverBlog(args);
                        break;
                    case "나가":
                        msgReturn = StopBot(author, args);
                        break;
                    /*case "구문분석":
                        try
                        {
                            //msgReturn = String.Format("결과: {0}", TokenizeSentence(args));
                            msgReturn = String.Format("미구현: {0}", args[1]);
                        }
                        catch (Exception ex)
                        {
                            msgReturn = ex.ToString();
                        }
                        break;
                    /*case "calc":
                        msgReturn = GetCalculator();
                        break;
                    case "날씨":
                        msgReturn = GetWeatherByLocation(args);
                        break;*/
                    case "?":
                    case "help":
                        msgReturn = GetHelp(args);
                        break;
                    default:
                        //msgReturn = "지원하지 않는 명령어 입니다. /? 또는 /help 를 입력하여 사용법을 확인하세요.";
                        try
                        {
                            if (args.Length > 1)
                            {
                                msgReturn = methods.ExecutePythonCommand(args[0], args[1]);
                            }
                            else
                            {
                                msgReturn = methods.ExecutePythonCommand(args[0], "");
                            }
                        }
                        catch (Exception)
                        {
                            msgReturn = String.Format("{0}: RPC 오류입니다. 잠시 후에 다시 시도해 주세요.", args[0]);
                        }

                        break;
                }
            }
            else
            {
                if (DateTime.Now - _settings.LastWarnedDate <= TimeSpan.FromSeconds(msgDelay))
                {
                    msgReturn = "";
                }
                else
                {
                    _settings.LastWarnedDate = DateTime.Now;
                    msgReturn = String.Format("도배 방지를 위해 {0}초에 한번만 쿼리 가능합니다.", msgDelay);
                }
            }

            return true;
        }

        /*private string TokenizeSentence(string[] args)
        {
            StringBuilder result = new StringBuilder();
            string sentence = args[1];

            var tokens = TwitterKoreanProcessorCS.Tokenize(sentence);

            foreach (var token in tokens)
            {
                result.AppendFormat(format: "{0}({1}) [{2},{3}] / ",
                    args: new object[] { token.Text, token.Pos.ToString(), token.Offset, token.Length });
            }

            return result.ToString();
        }*/

        private string ReturnBot()
        {
            return "일시중지를 해제합니다.";
        }

        private string StopBot(string author, string[] args)
        {
            if (_settings.LastBlocker != author || true) // 동일 유저 기능 잠시 사용 안함.
            {
                _settings.MessageDelayDefault = _settings.MessageDelay;
                _settings.MessageDelay = 600;
                _settings.LastWarnedDate = DateTime.Now;
                _settings.BotStopped = true;
                _settings.LastBlocker = author;

                return "10분간 봇의 작동을 중지합니다.";
            }
            else
            {
                return "동일한 유저가 연속으로 중지할 수 없습니다.";
            }
        }

        private string SearchNaverBlog(string[] args)
        {
            try
            {
                SearchEngine engine = new SearchEngine(_settings.NaverAPIClientID, _settings.NaverAPIClientSecret);
                SearchEngine.SearchResult result = engine.SearchNaverBlog(args[1]);

                string html;
                string desc;
                int maxLen;

                desc = result.items[0].description;

                if (desc.Length > 200)
                {
                    maxLen = 200;
                }
                else
                {
                    maxLen = desc.Length;
                }

                html = String.Format("{0}\r\n{1}\r\n{2}\r\n{3}", result.items[0].title, result.items[0].description.Substring(0, maxLen), result.items[0].link, result.items[0].postdate); ;

                html = WebUtility.HtmlDecode(html);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                return htmlDoc.DocumentNode.InnerText;
            }
            catch (Exception ex)
            {
                return "검색 엔진 오류.";
            }


        }

        private string LearnWord(string author, string requestedMessage)
        {
            BotAutoReply reply = new BotAutoReply(_settings.OracleURL, _settings.OraclePort, _settings.OracleDBName, _settings.OracleUserName, _settings.OracleUserPassword);

            try
            {
                requestedMessage = requestedMessage.Split(new[] { ' ' }, 2)[1];

                string[] args = requestedMessage.Split(new[] { '/' }, 2);

                args[0] = args[0].Trim();
                args[1] = args[1].Trim();

                if (args[0].Length == 0 || args[1].Length == 0)
                {
                    throw new Exception("No argument error");
                }

                // 금칙어 처리 (임시구현)
                /*if (args[0].Contains("ㅋ"))
                {
                    string wordCheck = args[0];

                    if (wordCheck.Replace("ㅋ", "").Length == 0)
                    {
                        return "학습이 불가능한 금칙어가 포함되어 있습니다.";
                    }
                }*/

                reply.RegisterMessage(author, args[0], args[1]);

                return String.Format("학습 완료: {0}, {1}", args[0], args[1]);
            }
            catch (Exception)
            {
                return "학습: 입력 포맷이 잘못되었습니다.\r\n사용법 예시: /학습 안녕/헬로";
            }
        }

        private string RetrieveAllKeywords()
        {
            BotAutoReply reply = new BotAutoReply(_settings.OracleURL, _settings.OraclePort, _settings.OracleDBName, _settings.OracleUserName, _settings.OracleUserPassword);

            return reply.RetrieveAllKeywords();
        }

        private string DeleteKeyword(string[] args)
        {
            BotAutoReply reply = new BotAutoReply(_settings.OracleURL, _settings.OraclePort, _settings.OracleDBName, _settings.OracleUserName, _settings.OracleUserPassword);

            if (args.Length > 1)
            {
                try
                {
                    reply.DeleteMessage(args[1]);

                    return String.Format("키워드 삭제 완료: {0}", args[1]);
                }
                catch (Exception)
                {
                    return String.Format("키워드삭제: '{0}' 삭제 중 오류가 발생하였습니다.", args[1]);
                }
            }

            return "오류: 입력 값 없음";
        }

        private string GetOffWorkTime(string[] args)
        {
            TimeSpan span;
            DateTime time = DateTime.Now;
            string result;

            time = time.AddHours(time.Hour * -1);
            time = time.AddMinutes(time.Minute * -1);
            time = time.AddSeconds(time.Second * -1);
            time = time.AddMilliseconds(time.Millisecond * -1);

            time = time.AddHours(18);

            span = time - DateTime.Now;

            if (span >= TimeSpan.Zero)
            {
                result = String.Format("퇴근 시간까지 {0}시간 {1}분 {2}초 남았습니다.", span.Hours, span.Minutes, span.Seconds);
            }
            else
            {
                result = String.Format("퇴근 시간으로부터 {0}시간 {1}분 {2}초 경과하였습니다. 퇴근하셨습니다.", -1 * span.Hours, -1 * span.Minutes, -1 * span.Seconds);
            }

            return result;
        }

        /*private PipeClient ConnectPipe()
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
        }*/

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
            return "0.0.3 Beta";
        }

        private string GetHelp(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("사용법\r\n");
            sb.AppendFormat("/말해 <문장>: 입력한 말을 반복\r\n");
            sb.AppendFormat("/날씨 <위치>: 특정 위치의 현재 날씨 출력\r\n");
            sb.AppendFormat("/검색 <키워드>: 네이버 블로그 검색 결과 출력\r\n");
            sb.AppendFormat("/시간: 현재 시간 출력\r\n");
            sb.AppendFormat("/퇴근시간: 남은 퇴근 시간 출력\r\n");
            sb.AppendFormat("/학습 <키워드1>/<키워드2>: <키워드1>을 입력 시 <키워드2>를 출력\r\n");
            sb.AppendFormat("/키워드: 학습한 키워드 목록 출력\r\n");
            sb.AppendFormat("/키워드삭제 <키워드1>: 학습한 키워드 삭제\r\n");
            sb.AppendFormat("/나가: 도배 방지 기능. 10분간 작동 정지.\r\n");
            sb.AppendFormat("/돌아와: 일시 정지 해제.\r\n");
            //sb.AppendFormat("/구문분석 <문장>: 문장 구문 분석\r\n");
            sb.AppendFormat("/version: 현재 버전 출력\r\n");
            sb.AppendFormat("※ 설명서에 적혀 있는 꺽쇠 < > 는 입력하지 않음.");

            return sb.ToString();
        }
    }
}
