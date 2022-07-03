using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class Answer : AuditableEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public string Content { get; set; }
    }
}