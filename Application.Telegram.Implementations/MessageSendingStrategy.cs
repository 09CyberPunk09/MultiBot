using Application.Chatting.Core.Repsonses;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static Application.Chatting.Core.Repsonses.Button;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.Telegram.Implementations;

public class MessageSendingStrategy
{
    private readonly ITelegramBotClient _uiClient;
    public MessageSendingStrategy(ITelegramBotClient client)
    {
        _uiClient = client;
    }
    public async Task SendMessage(ContentResultV2 result)
    {
        var contentResult = result as AdressedContentResult;
        bool edited = contentResult.Edited;
        bool hasPhoto = contentResult.Photo != null;
        InputOnlineFile photo = null;
        if (hasPhoto)
        {
            if (contentResult.Photo.Mode == PhotResultMode.Url)
            {
                using WebClient client = new WebClient();
                byte[] data = client.DownloadData(contentResult.Photo.Url);
                MemoryStream mem = new(data);
                photo = new InputOnlineFile(mem);
                //await _uiClient.SendPhotoAsync(
                //  chatId: chatId,
                //  caption: message.Text,
                //  photo: t,
                //  parseMode: ParseMode.Html);
            }
        }

        bool hasMenu = contentResult.Menu != null;
        IReplyMarkup menu = null;
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
                    menu = new ReplyKeyboardMarkup(menuData.Select(x => x.Select(y => new KeyboardButton(y.Text))));
                    break;
                default:
                    break;
            }
        }

        //if (edited)
        //{
        //    var msgId = await _uiClient.Edi
        //}
        /*else*/
        if (hasPhoto)
        {
            var msgId = await _uiClient.SendPhotoAsync(
                chatId: contentResult.ChatId,
                caption: contentResult.Text,
                photo: photo,
                replyMarkup: menu,
                parseMode: ParseMode.Html);
        }
        else
        {
            var msgId = await _uiClient.SendTextMessageAsync(
                chatId: contentResult.ChatId,
                text: contentResult.Text,
                replyMarkup: menu);
        }

    }
}
