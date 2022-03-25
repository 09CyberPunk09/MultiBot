using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.ResponseTypes
{
    public static class ResponseTemplates
    {
        public static ContentResult Text(string text)
        {
            //todo: implement delayd messages and fire-and-forget messages
            return new() { Text = text };
        }

        public static ContentResult YesNo(string question)
        {
            var markups = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Yes", true.ToString()),
                InlineKeyboardButton.WithCallbackData("No", false.ToString())
            };

            return new ContentResult()
            {
                Text = question,
                Buttons = new InlineKeyboardMarkup(markups.ToArray())
            };
        }
    }
}