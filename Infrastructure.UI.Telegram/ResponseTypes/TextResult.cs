using Infrastructure.TextUI.Core.Interfaces;

namespace Infrastructure.TelegramBot.ResponseTypes
{
    public class TextResult : ContentResult
    {
        public TextResult()
        {
        }

        public TextResult(string message, long reciipientChatId = 0)
        {
            Text = message;
            RecipientChatId = reciipientChatId;
        }
    }
}