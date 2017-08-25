using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TelegramBot.Util.Bot;

namespace TelegramBot.Util.PostCommand
{
    public class MessageKeeper
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public MessageKeeper()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("test");
        }

        public void SaveMessage(ChatMessage msgInfo, string tableName)
        {
            var col = _database.GetCollection<BsonDocument>(tableName);

            var doc = new BsonDocument
            {
                {
                    "sender", new BsonDocument
                    {
                        {
                            "sender_id", msgInfo.Message.From.ID
                        },
                        {
                            "first_name", msgInfo.Message.From.FirstName
                        },
                        {
                            "last_name", msgInfo.Message.From.LastName
                        }
                    }
                },
                {
                    "chat_id", msgInfo.Message.Chat.ID
                },
                {
                    "message", msgInfo.Message.Text
                },
                {
                    "date", msgInfo.Message.Date
                }
            };

            col.InsertOne(doc);
        }
    }
}
