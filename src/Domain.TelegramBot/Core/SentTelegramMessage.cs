using Application.Chatting.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Message = Telegram.Bot.Types.Message;

namespace Application.TelegramBot.Commands.Core;

public class SentTelegramMessage : SentMessageRepsonse
{
    public Message SentMessage { get; set; }
}
