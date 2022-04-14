using Common.BaseTypes;

namespace Common.Entites
{
    public class PredefinedAnswer : AuditableEntity
    {
        public string Content { get; set; }
        public Question Question { get; set; }
    }
}