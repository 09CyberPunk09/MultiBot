using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using System.Collections.Generic;

namespace Infrastructure.TelegramBot.ResponseTypes
{
    public class MultiMessageResult : ContentResult
    {
        public List<ContentResult> Messages { get; set; }
        public string Text { get; set; }
    }
}