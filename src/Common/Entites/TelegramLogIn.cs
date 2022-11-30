using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class TelegramLogIn : AuditableEntity
    {
        public Guid Id { get; set; }
        public long TelegramUserId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
