using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.ChatEngine.Commands.Repsonses;
using static TelegramBot.ChatEngine.Commands.Repsonses.Button;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.Telegram.Implementations;

public class MessageSendingStrategy
{
    private readonly ITelegramBotClient _uiClient;
    public MessageSendingStrategy(ITelegramBotClient client)
    {
        _uiClient = client;
    }
    public async Task<Message> SendMessage(ContentResultV2 result)
    {
        Message messageResponse = default;
        var contentResult = result as AdressedContentResult;
        bool edited = contentResult.Edited;
        bool hasPhoto = contentResult.Photo != null;
        InputFile photo = null;
        //photo preparation
        if (hasPhoto)
        {
            if (contentResult.Photo.Mode == PhotResultMode.Url)
            {
                using WebClient client = new WebClient();
                byte[] data = client.DownloadData(contentResult.Photo.Url);
                MemoryStream mem = new(data);
                photo = new InputFile(mem);
                await _uiClient.SendPhotoAsync(
                  chatId: chatId,
                  caption: message.Text,
                  photo: t,
                  parseMode: ParseMode.Html);
            }
        }

        bool hasMenu = contentResult.Menu != null;
        IReplyMarkup menu = null;
        //menu preparation
        if (hasMenu)
        {
            var type = contentResult.Menu.Type;
            var menuData = contentResult.Menu.MenuScheme;
            switch (type)
            {
                case MenuType.MessageMenu:
                    menu = new InlineKeyboardMarkup(menuData.Select(x => x.Select(y =>
                    {
                        switch (y.Type)
                        {
                            case ButtonContentType.Text:
                                return InlineKeyboardButton.WithCallbackData(y.Text, y.Text);
                            case ButtonContentType.Url:
                                return InlineKeyboardButton.WithUrl(y.Text, y.Url);
                            case ButtonContentType.CallbackData:
                                return InlineKeyboardButton.WithCallbackData(y.Text, y.CallbackData);
                            default:
                                throw new NotImplementedException();
                        }
                    })));
                    break;
                case MenuType.MenuKeyboard:
                    menu = new ReplyKeyboardMarkup(menuData.Select(x => x.Select(y => new KeyboardButton(y.Text))))
                    {
                        ResizeKeyboard = true
                    };
                    break;
                default:
                    break;
            }
        }

        if (edited)
        {
            messageResponse = await _uiClient.EditMessageTextAsync(
                contentResult.ChatId,
                contentResult.LastBotMessageId.Value,
                contentResult.Text,
                replyMarkup: menu as InlineKeyboardMarkup);
            return messageResponse;
        }
        /*else*/
        if (hasPhoto)
        {
            messageResponse = await _uiClient.SendPhotoAsync(
                chatId: contentResult.ChatId,
                caption: contentResult.Text,
                photo: photo,
                replyMarkup: menu,
                parseMode: ParseMode.Html);
        }
        else
        {
            try
            {
                messageResponse = await _uiClient.SendTextMessageAsync(
                chatId: contentResult.ChatId,
                text: contentResult.Text,
                replyMarkup: menu);
            }
            catch (Exception ex)
            {

            };
        }
        return messageResponse;
    }
}
