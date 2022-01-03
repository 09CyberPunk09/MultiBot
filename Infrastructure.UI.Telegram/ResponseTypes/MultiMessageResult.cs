using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using System.Collections.Generic;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
    public class MultiMessageResult : ContentResult
    {
        public List<BotMessage> Messages { get; set; }
        public string Text { get; set; }
    }
}
