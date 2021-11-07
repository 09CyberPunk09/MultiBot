using Infrastructure.UI.Core.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
    public class MultiMessageResult : ContentResult
	{
		public List<BotMessage> Messages { get; set; }
        public string Text { get; set; }
    }
}
