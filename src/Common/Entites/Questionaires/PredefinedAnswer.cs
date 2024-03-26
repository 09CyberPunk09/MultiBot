using Common.BaseTypes;
using System;

namespace Common.Entites.Questionaires;

public class PredefinedAnswer : AuditableEntity
{
    public Guid QuestionId { get; set; }
    public string Text { get; set; }
}
