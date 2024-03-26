using Application.Chatting.Core.Caching;
using Application.Chatting.Core.Messaging;
using Application.Chatting.Core.Repsonses;
using Common.Dto;

namespace Application.Chatting.Core.Context;

public class MessageContext
{
    public virtual RepsonseHelper Response { get; set; } = new();
    public virtual ShortUserInfoDto User { get; set; }
    public virtual Message Message { get; init; }
    public virtual PipelineContext PipelineContext { get; set; }
    public virtual CachedChatDataWrapper Cache { get; set; }
}
