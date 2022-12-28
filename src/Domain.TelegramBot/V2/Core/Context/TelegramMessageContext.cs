using Application.TextCommunication.Core.Context;

namespace Application.TelegramBot.Pipelines.V2.Core.Context
{
    public class TelegramMessageContext : MessageContext
    {
        public long RecipientChatId { get; set; }
        public long RecipientUserId { get; set; }
    }
}
