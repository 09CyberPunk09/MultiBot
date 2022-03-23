using Common.BaseTypes;

namespace Common.Entites
{
    public class Answer : AuditableEntity
    {
        public Question Question { get; set; }
        public string Content { get; set; }

    }
}
