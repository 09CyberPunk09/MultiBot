using Persistence.Core.BaseTypes;

namespace Persistence.Sql.Entites
{
    //todo: divide the entity on two separate: base data and app-specific settings
    public class User : AuditableEntity
    {
        public long? TelegramUserId { get; set; }
        public bool TelegramLoggedIn { get; set; }
        public string Name { get; set; }
    }
}
