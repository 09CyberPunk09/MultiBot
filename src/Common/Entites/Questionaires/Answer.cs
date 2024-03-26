using Common.BaseTypes;
using System;

namespace Common.Entites.Questionaires;

public class Answer : AuditableEntity
{
    public Guid QuestionaireSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public string Text { get; set; }
}
