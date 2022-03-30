using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;

namespace Infrastructure.TelegramBot.ResponseTypes
{
    public class MultiMessageResult : ContentResult
    {
        public List<ContentResult> Messages { get; set; }
        public string Text { get; set; }
    }
}