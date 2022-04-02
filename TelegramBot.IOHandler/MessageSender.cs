using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.IOHandler
{
    internal class MessageSender : IResultSender
    {
        private ITelegramBotClient _uiClient;

        //TODO: Move caching to handlers
        private readonly TelegramCache cache = new();

        public MessageSender(ITelegramBotClient uiClient)
        {
            _uiClient = uiClient;
        }

        public async Task SendMessage(ContentResult contentResult)
        {
            var chatId = new ChatId(contentResult.RecipientChatId);
            string lastMessageCacheey = "LastPipelineMessageId";
            async Task<Telegram.Bot.Types.Message> SendTextMessageAsync(string text = "", IReplyMarkup markup = null)
            {
                var message = await _uiClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: markup
                );

                if(contentResult.Menu != null)
                {
                    await _uiClient.SendTextMessageAsync(
                        text: contentResult.Text, 
                        chatId: chatId,
                        replyMarkup: contentResult.Menu);
                         
                }

                cache.SetValueForChat(lastMessageCacheey, message.MessageId, contentResult.RecipientChatId);
                return message;
            }

            async Task EditMessageAsync(ContentResult message)
            {
                int lastMessageId = cache.GetValueForChat<int>(lastMessageCacheey, message.RecipientChatId);
                await _uiClient.EditMessageTextAsync(chatId, lastMessageId, message.Text);
                await _uiClient.EditMessageReplyMarkupAsync(chatId, lastMessageId, message.Buttons);
            }

            if (contentResult.Edited)
            {
                await EditMessageAsync(contentResult);
            }
            else if (contentResult.MultiMessages != null)
            {
                await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
                contentResult.MultiMessages.ForEach(async x => await SendTextMessageAsync(x.Text));
            }
            else
            {
                await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
            }
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