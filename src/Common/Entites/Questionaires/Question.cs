using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites.Questionaires;

public class Question : AuditableEntity
{
    public int Position { get; set; }
    public Guid QuestionaireId { get; set; }
    public Questionaire Questionaire { get; set; }
    public string Text { get; set; }
    public AnswerType AnswerType { get; set; }
    public int? RangeMin { get; set; }
    public int? RangeMax { get; set; }
    public List<PredefinedAnswer> PredefinedAnswers { get; set; }
}
