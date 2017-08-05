﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TelegramBot.Util.Bot;
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

            ChatMessage msg;
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

                    bot = new BotCommander(_settings);

                    string msgReturn;
                    
                    if (bot.ProcessCommand(msg.Message.Text, out msgReturn))
                    {
                        //Bot.SendTextMessageAsync(msg.Message.Chat.ID, msgReturn);
                        _comm.SendMessage(msg.Message.Chat.ID, msgReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                Bot.SendTextMessageAsync(defaultRcpt, ex.ToString());
                Bot.SendTextMessageAsync(defaultRcpt, content);
            }
        }
    }
}