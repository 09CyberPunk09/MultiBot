using Infrastructure.UI.Core.Interfaces;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.Core.Types
{
    public static class ResponseTemplates
    {
        public static ContentResult Text(string text)
        {
            //todo: implement delayd messages and fire-and-forget messages
            return new() { Text = text };
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
