using System;

namespace Application.Services.Questions.Dto
{
    public class AnswerQuestionDto
    {
        public Guid QuesiotnId { get; set; }
        public Guid? PredefinedAnswerId { get; set; }
        public string Text { get; set; }
    }
}
