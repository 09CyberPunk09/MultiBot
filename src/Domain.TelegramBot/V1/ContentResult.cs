using System.Collections.Generic;
using Application.TextCommunication.Core.Repsonses;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class ContentResult
    {
        public ContentResult()
        {
        }
        public PhotoResult Photo { get; set; }
        public string Text { get; set; }
        public virtual bool InvokeNextImmediately { get; set; } = false;
        public long RecipientChatId { get; set; }
        public bool Edited { get; set; }
        public InlineKeyboardMarkup Buttons { get; set; }
        public List<ContentResult> MultiMessages { get; set; }
        public ReplyKeyboardMarkup Menu { get; set; }
        public bool IsEmpty { get; set; } = false;
        public static ContentResult Empty = new ContentResult() { IsEmpty = true };
    }
}