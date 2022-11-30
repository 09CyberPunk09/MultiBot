using Common.Entites;
using System;
using TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class MessageContext
    {
        public User User { get; set; }
        public MessageContext(long recipientChatId)
        {
            RecipientChatId = recipientChatId;
        }
        public Message Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public long RecipientChatId { get; }
        public long RecipientUserId { get; set; }
        public PipelineInfo CurrentPipeline { get; set; }

    }
}