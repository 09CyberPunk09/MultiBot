using Infrastructure.TextUI.Core.Types;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot
{
    public class MessageSender : IResultSender
    {
        ITelegramBotClient _uiClient;
        private readonly TelegramCache cache = new();

        public MessageSender(ITelegramBotClient uiClient)
        {
            _uiClient = uiClient;
        }
        public async Task SendMessage(ContentResult message, long recipient)
        {
            var chatId = new ChatId(recipient);
            string lastMessageCacheey = "LastPipelineMessageId";
            async Task<Telegram.Bot.Types.Message> SendTextMessageAsync(string text = "", IReplyMarkup markup = null)
            {
                var message = await _uiClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: markup);

                cache.SetValueForChat(lastMessageCacheey, message.MessageId, recipient);
                return message;
            }

            async Task EditMessageAsync(EditLastMessage message)
            {
                int lastMessageId = cache.GetValueForChat<int>(lastMessageCacheey, recipient);
                await _uiClient.EditMessageTextAsync(chatId, lastMessageId, message.NewMessage.Text);
                await _uiClient.EditMessageReplyMarkupAsync(chatId, lastMessageId, message.NewMessage.Buttons);
            }


            //TODO: Rewrite to more extensible way
            switch (message)
            {
                case EditLastMessage editedMessage:
                    await EditMessageAsync(editedMessage);
                    break;
                case TextResult textResult:
                    await SendTextMessageAsync(textResult.Text);
                    break;
                case MultiMessageResult multi:
                    multi.Messages.ForEach(async x => await SendTextMessageAsync(x.Text));
                    break;
                case BotMessage botMessage:
                    await SendTextMessageAsync(botMessage.Text, botMessage.Buttons);
                    break;
                case ContentResult textResult:
                    await SendTextMessageAsync(textResult.Text);
                    break;
                default:
                    throw new NotImplementedException("The method to concrete response type is not implemented!");
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
