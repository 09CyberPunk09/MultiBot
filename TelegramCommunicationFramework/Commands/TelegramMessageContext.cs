using TelegramBot.ChatEngine.Commands.Caching;
using TelegramBot.ChatEngine.Commands.Context;
using TelegramBot.ChatEngine.Commands.Dto;
using TelegramBot.ChatEngine.Commands.Messaging;
using TelegramBot.ChatEngine.Commands.Repsonses;

namespace TelegramBot.ChatEngine.Commands;
/// <summary>
/// Contains all necessary data for a command
/// </summary>
public class TelegramMessageContext
{
    public virtual RepsonseHelper Response { get; set; } = new();
    public virtual ShortUserInfoDto User { get; set; }
    public virtual Message Message { get; init; }
    public virtual PipelineContext PipelineContext { get; set; }
    public virtual CachedChatDataWrapper Cache { get; set; }
    public long RecipientChatId { get; set; }
    public long RecipientUserId { get; set; }
}
