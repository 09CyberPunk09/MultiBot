using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class User : AuditableEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public long? TelegramChatId { get; set; }
        public bool TelegramLoggedIn { get; set; }
        public bool IsSpecial { get; set; }
        public Guid? LastTimeTrackingEntry { get; set; }
    }
}