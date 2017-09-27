using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TelegramBot.Util.Bot;
using TelegramBot.Util.PostCommand;
using TelegramBot.Util.Settings;

namespace TelegramBot.MainListener
{
    public partial class MsgHandlerMain : System.Web.UI.Page
    {
        SettingsManager _settings;
        MessageCommunicator _comm;

        protected void Page_Load(object sender, EventArgs e)
        {
            _settings = new SettingsManager("CheongBot");
            _comm = new MessageCommunicator(_settings.BotToken);
            
            var Bot = new Telegram.Bot.TelegramBotClient(_settings.BotToken);

            ChatMessage msg = null;
            BotCommander bot;
            string content = "";
            long defaultRcpt = _settings.DefaultRcpt;
            
            try
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    content = reader.ReadToEnd();
                    
                    msg = ChatMessage.FromJsonString(content);

                    defaultRcpt = msg.Message.Chat.ID;

                    // Save message before the process (only for registered room)
                    if (msg.Message.Chat.ID == _settings.RegisteredRoom1)
                    {
                        SaveMessage(msg, "messages");
                    }
                    else if (msg.Message.Chat.ID == _settings.RegisteredRoom2)
                    {
                        SaveMessage(msg, "test.messages");
                    }

                    bot = new BotCommander(_settings);

                    string msgReturn;
                    
                    if (bot.ProcessCommand(msg.Message.From.ToString(), msg.Message.Text, out msgReturn))
                    {
                        //Bot.SendTextMessageAsync(msg.Message.Chat.ID, msgReturn);
                        _comm.SendMessage(msg.Message.Chat.ID, msgReturn);
                    }
                    else
                    {
                        BotAutoReply botAutoReply;
                        botAutoReply = new BotAutoReply(_settings.OracleURL, _settings.OraclePort, _settings.OracleDBName, _settings.OracleUserName, _settings.OracleUserPassword);

                        string reply = botAutoReply.FindMessage(msg.Message.Text);

                        if (reply != "")
                        {
                            _comm.SendMessage(msg.Message.Chat.ID, reply);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bot.SendTextMessageAsync(defaultRcpt, ex.ToString());
                Bot.SendTextMessageAsync(defaultRcpt, content);
            }
        }

        private void SaveMessage(ChatMessage msgInfo, string tableName)
        {
            try
            {
                MessageKeeper keeper = new MessageKeeper();

                if (msgInfo != null)
                {
                    keeper.SaveMessage(msgInfo, tableName);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}