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

        //TODO: Move caching to handlers
        private readonly TelegramCache cache = new();

        public MessageSender(ITelegramBotClient uiClient)
        {
            _uiClient = uiClient;
        }

        public async Task SendMessage(ContentResult contentResult)
        {
            bool hasPhoto = contentResult.Photo != null;
            bool hasText = !string.IsNullOrEmpty(contentResult.Text);
            bool hasInlineButtons = contentResult.Buttons != null;
            bool hasMenu = contentResult.Menu != null;
            bool isEdited = contentResult.Edited;
            bool hasMultiMessages = contentResult.MultiMessages != null;

            //validation
            if (!hasText && !hasMenu)
            {
                //throw new Exception("you are trying to send an empty message");
                Console.WriteLine("you are trying to send an empty message");
                return;
            }

            var chatId = new ChatId(contentResult.RecipientChatId);
            string lastMessageCacheey = "LastPipelineMessageId";

            async Task<Telegram.Bot.Types.Message> SendTextMessageAsync(string text = "", IReplyMarkup markup = null)
            {
                var message = await _uiClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: markup
                );

                //if (contentResult.Menu != null)
                //{
                //    await _uiClient.SendTextMessageAsync(
                //        text: "Menu updated",
                //        chatId: chatId,
                //        replyMarkup: contentResult.Menu);
                //}

                cache.SetValueForChat(lastMessageCacheey, message.MessageId, contentResult.RecipientChatId);
                return message;
            }

            async Task EditMessageAsync(ContentResult message)
            {
                int lastMessageId = cache.GetValueForChat<int>(lastMessageCacheey, message.RecipientChatId);
                await _uiClient.EditMessageTextAsync(chatId, lastMessageId, message.Text);
                await _uiClient.EditMessageReplyMarkupAsync(chatId, lastMessageId, message.Buttons);
            }

            async Task SendPhotoAsync(ContentResult message)
            {
                switch (message.Photo.Mode)
                {
                    case PhotResultMode.Url:
                        await _uiClient.SendPhotoAsync(
                            chatId: chatId,
                            caption: message.Text,
                            replyMarkup: message.Menu,
                            photo: message.Photo.Url,
                            parseMode: ParseMode.Html);
                        break;

                    case PhotResultMode.Content:
                        MemoryStream mem = new(message.Photo.PhotoContent);
                        var t = new InputOnlineFile(mem);
                        await _uiClient.SendPhotoAsync(
                          chatId: chatId,
                          caption: message.Text,
                          photo: t,
                          parseMode: ParseMode.Html);
                        break;

                    default:
                        break;
                }
            }

            try
            {
                if (contentResult.IsEmpty)
                    return;

                if (hasPhoto)
                {
                    await SendPhotoAsync(contentResult);
                    return;
                }
                else if (contentResult.Edited)
                {
                    await EditMessageAsync(contentResult);
                    return;
                }
                if (contentResult.MultiMessages != null)
                {
                    await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
                    contentResult.MultiMessages.ForEach(async x => await SendTextMessageAsync(x.Text, x.Buttons));
                    return;
                }

                if (hasInlineButtons && hasMenu)
                {
                    await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
                    await SendTextMessageAsync("Menu updated", contentResult.Menu);
                    return;
                }
                else if (hasMenu)
                {
                    await SendTextMessageAsync("Menu updated", contentResult.Menu);
                    return;
                }
                else if (hasInlineButtons)
                {
                    await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
                    return;
                }

                await SendTextMessageAsync(contentResult.Text, contentResult.Buttons);
            }
            catch (Exception ex)
            {
                throw;
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