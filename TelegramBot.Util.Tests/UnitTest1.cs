using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelegramBot.Util.Bot;

namespace TelegramBot.Util.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Settings.SettingsManager settings = new Settings.SettingsManager("CheongBot");

            BotAutoReply reply;
            reply = new BotAutoReply(settings.OracleURL, settings.OraclePort, settings.OracleDBName, settings.OracleUserName, settings.OracleUserPassword);

            reply.RegisterMessage("chlee", "가나다", "빠가");

            reply.FindMessage("가나다");

        }

        [TestMethod]
        public void TestLearnWord()
        {
            BotCommander commander = new BotCommander(new Settings.SettingsManager("CheongBot"));

            string outstring;

            commander.ProcessCommand("chlee", "/학습 가나다/다라마", out outstring);

            Console.WriteLine(outstring);
        }

        [TestMethod]
        public void TestKeyWord()
        {
            Settings.SettingsManager settings = new Settings.SettingsManager("CheongBot");
            BotAutoReply reply;

            reply = new BotAutoReply(settings.OracleURL, settings.OraclePort, settings.OracleDBName, settings.OracleUserName, settings.OracleUserPassword);

            string s = reply.RetrieveAllKeywords();
        }
    }
}
