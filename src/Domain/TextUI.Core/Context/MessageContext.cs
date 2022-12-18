using Application.TextCommunication.Core.Caching;
using Application.TextCommunication.Core.Messaging;
using Common.Dto;

namespace Application.TextCommunication.Core.Context;

public class MessageContext
{
    public ShortUserInfoDto User { get; set; }
    public Message Message { get; init; }
    public PipelineContext PipelineContext { get; set; }
    public CachedChatDataWrapper Cache { get; set; }
}
