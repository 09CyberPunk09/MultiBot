using Application.Chatting.Core.Messaging;
using Message = Telegram.Bot.Types.Message;

namespace Application.TelegramBot.Commands.Core;

public class SentTelegramMessage : SentMessageRepsonse
{
    public Message SentMessage { get; set; }
}
