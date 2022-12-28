using Application.Chatting.Core.Context;

namespace Application.TelegramBot.Commands.Core.Context
{
    public class TelegramMessageContext : MessageContext
    {
        public long RecipientChatId { get; set; }
        public long RecipientUserId { get; set; }
    }
}
