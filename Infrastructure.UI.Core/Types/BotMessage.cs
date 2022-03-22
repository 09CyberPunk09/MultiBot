using Infrastructure.TextUI.Core.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TextUI.Core.Types
{
    public class BotMessage : ContentResult
    {
        public string Text { get; set; }
        public InlineKeyboardMarkup Buttons { get; set; }
    }
}
