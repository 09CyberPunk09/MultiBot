using TelegramBot.ChatEngine.Commands.Dto;

namespace TelegramBot.ChatEngine.Commands;

/// <summary>
/// Incoming telegram message
/// </summary>
public class TelegramMessage
{
    public string Text { get; set; }
    public long ChatId { get; set; }
    public long UserId { get; set; }
    public List<UploadedFileDto> Files { get; set; }
}