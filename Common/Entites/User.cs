using Common.BaseTypes;
using System;

namespace Common.Entites
{
    //todo: divide the entity on two separate: base data and app-specific settings
    public class User : AuditableEntity
    {
        public long? TelegramChatId { get; set; }
        public bool TelegramLoggedIn { get; set; }
        public string Name { get; set; }
        public bool IsSpecial { get; set; }
        public Guid? LastTimeTrackingEntry { get; set; }
    }
}