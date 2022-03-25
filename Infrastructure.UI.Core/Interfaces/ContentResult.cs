using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TextUI.Core.Interfaces
{
    public class ContentResult
    {
        public string Text { get; set; }
        public virtual bool InvokeNextImmediately { get; set; } = false;
        public long RecipientChatId { get; set; }
        public bool Edited { get; set; }
        public InlineKeyboardMarkup Buttons { get; set; }
        public List<ContentResult> MultiMessages { get; set; }

    }
}