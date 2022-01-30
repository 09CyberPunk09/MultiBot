using Persistence.Core.BaseTypes;
using System;
using System.Collections.Generic;

namespace Persistence.Sql.Entites
{
    public class Question : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public bool AnswersAsInt { get; set; }
        public bool HasPredefinedAnswers { get; set; }
        public List<PredefinedAnswer> PredefinedAnswers { get; set; } = new();
        public List<Answer> Answers { get; set; } = new();
        public string CronExpression { get; set; }
    }
}
