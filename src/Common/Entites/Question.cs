using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites
{
    public class Question : SchedulerInfoEntity
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