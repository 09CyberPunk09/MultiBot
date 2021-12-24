using Infrastructure.UI.Core.Interfaces;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
    public class TextResult : ContentResult
	{
        public TextResult()
        {

        }
        public TextResult(string message)
        {
            Text = message;
        }
	}
}
