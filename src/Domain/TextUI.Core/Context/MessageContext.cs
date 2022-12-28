using Application.TextCommunication.Core.Caching;
using Application.TextCommunication.Core.Messaging;
using Application.TextCommunication.Core.Repsonses;
using Common.Dto;

namespace Application.TextCommunication.Core.Context;

public class MessageContext
{
    public virtual RepsonseHelper Response { get; set; } = new();
    public virtual ShortUserInfoDto User { get; set; }
    public virtual Message Message { get; init; }
    public virtual PipelineContext PipelineContext { get; set; }
    public virtual CachedChatDataWrapper Cache { get; set; }
}
