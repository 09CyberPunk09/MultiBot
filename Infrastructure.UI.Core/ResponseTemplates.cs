using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TextUI.Core
{
    //todo: remove cope from core
    public static class ResponseTemplates
    {
        public static ContentResult Text(string text, bool invokeNextImmediately = false)
        {
            //todo: implement delayd messages and fire-and-forget messages
            return new()
            {
                Text = text,
                InvokeNextImmediately = invokeNextImmediately
            };
        }

        public static BotMessage YesNo(string question)
        {
            var markups = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Yes", true.ToString()),
                InlineKeyboardButton.WithCallbackData("No", false.ToString())
            };

            return new BotMessage()
            {
                Text = question,
                Buttons = new InlineKeyboardMarkup(markups.ToArray())
            };
        }
    }
}