using Common.BaseTypes;
using System;

namespace Common.Entites.Questionaires;

public class QuestionaireSession : AuditableEntity
{
    public Guid QuestionaireId { get; set; }
    public Questionaire Questionaire { get; set; }

}
