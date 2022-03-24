using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using System.Collections.Generic;

namespace Infrastructure.TelegramBot.ResponseTypes
{
    public class MultiMessageResult : ContentResult
    {
        public List<BotMessage> Messages { get; set; }
#pragma warning disable CS0108 // 'MultiMessageResult.Text' hides inherited member 'ContentResult.Text'. Use the new keyword if hiding was intended.
        public string Text { get; set; }
#pragma warning restore CS0108 // 'MultiMessageResult.Text' hides inherited member 'ContentResult.Text'. Use the new keyword if hiding was intended.
    }
}