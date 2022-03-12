using Infrastructure.UI.Core.MessagePipelines;
using System;

namespace Infrastructure.UI.Core.Types
{
    public class MessageContext
    {
        public bool PipelineStageSucceeded { get; set; } = true;
        public Message Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool MoveNext { get; set; }
        public bool PipelineEnded { get; set; } = false;
        public long Recipient { get; set; }
        public long RecipientUserId { get; set; }
        public Stage CurrentStage { get; set; }  
    }
}
