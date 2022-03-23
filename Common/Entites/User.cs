using Common.BaseTypes;

namespace Common.Entites
{
    //todo: divide the entity on two separate: base data and app-specific settings
    public class User : AuditableEntity
    {
        public long? TelegramUserId { get; set; }
        public long? TelegramChatId { get; set; }
        public bool TelegramLoggedIn { get; set; }
        public string Name { get; set; }
        public bool IsSpecial { get; set; }
    }
}
