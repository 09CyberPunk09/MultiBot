using Persistence.Core.BaseTypes;

namespace Persistence.Sql.Entites
{
    public class Answer : AuditableEntity
    {
        public Question Question { get; set; }
        public string Content { get; set; }

    }
}
