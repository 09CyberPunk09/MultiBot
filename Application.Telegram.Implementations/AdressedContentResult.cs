using Application.Chatting.Core.Repsonses;

namespace Application.Telegram.Implementations;

public class AdressedContentResult : ContentResultV2
{
    public long? ChatId { get; set; }
    public int? LastBotMessageId { get; set; }
}
