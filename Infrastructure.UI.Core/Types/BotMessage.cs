using Infrastructure.TextUI.Core.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TextUI.Core.Types
{
    public class BotMessage : ContentResult
    {
#pragma warning disable CS0108 // 'BotMessage.Text' hides inherited member 'ContentResult.Text'. Use the new keyword if hiding was intended.
        public string Text { get; set; }
#pragma warning restore CS0108 // 'BotMessage.Text' hides inherited member 'ContentResult.Text'. Use the new keyword if hiding was intended.
        public InlineKeyboardMarkup Buttons { get; set; }
    }
}