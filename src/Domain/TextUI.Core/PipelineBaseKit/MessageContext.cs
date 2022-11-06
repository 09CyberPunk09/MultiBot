using Common.Entites;
using Persistence.Master;
using System;
using System.Linq;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class MessageContext
    {
        public User User { get; }
        public MessageContext(long recipientChatId)
        {
            RecipientChatId = recipientChatId;
            //todo: implement getting from cache
            using (var ctx = new LifeTrackerDbContext())
            {
                User = ctx.Users.FirstOrDefault(u => u.TelegramChatId.HasValue && u.TelegramChatId == recipientChatId);
            }
        }
        public bool PipelineStageSucceeded { get; set; } = true;
        public Message Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public long RecipientChatId { get; }
        public long RecipientUserId { get; set; }
        public Stage CurrentStage { get; set; }

    }
}