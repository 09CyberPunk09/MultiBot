using System.Collections.Generic;
using TelegramBot.ChatEngine.Commands.Dto;

namespace TelegramBot.ChatEngine.Commands.Messaging;

public class Message
{
    public string Text { get; set; }
    public List<UploadedFileDto> Files { get; set; }
}
