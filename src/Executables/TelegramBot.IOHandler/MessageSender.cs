using Application.Telegram.Implementations;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TextUI.Core.PipelineBaseKit;

namespace LifeTracker.TelegramBot.IOHandler
{
    internal class MessageSender
    {
        private ITelegramBotClient _uiClient;
        public MessageSender(ITelegramBotClient uiClient)
        {
            _uiClient = uiClient;
        }

        public async Task SendMessage(ContentResult contentResult)
        {
            var messageSendingStrategy = new MessageSendingStrategy(_uiClient);
            await messageSendingStrategy.SendMessage(contentResult);
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}