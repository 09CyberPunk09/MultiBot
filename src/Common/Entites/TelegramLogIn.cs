using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class TelegramLogIn : AuditableEntity
    {
        public long TelegramUserId { get; set; }
        public long TelegramChatId { get; set; }
        public Guid UserId { get; set; }
    }
}
