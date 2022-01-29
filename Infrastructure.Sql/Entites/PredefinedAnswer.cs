using Persistence.Core.BaseTypes;

namespace Persistence.Sql.Entites
{
    public class PredefinedAnswer : AuditableEntity
    {
        public string Content { get; set; }
        public Question Question { get; set; }
    }
}
