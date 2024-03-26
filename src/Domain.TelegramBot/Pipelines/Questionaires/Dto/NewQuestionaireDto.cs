using Common.Entites.Questionaires;
using System.Collections.Generic;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;


internal class NewQuestionaireDto
{
    public string Name { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();

}
internal class QuestionDto
{
    //for deserialization
    public QuestionDto() { }
    public QuestionDto(AnswerType type)
    {
        AnswerType = type;
    }
    public string Text { get; set; }
    public AnswerType AnswerType { get; set; } = AnswerType.WithoutPredefinedAnswers;
    public List<PredefinedAnswerDto> PredefinedAnswers { get; set; } = new();
    public (int?, int?) NumericRange { get; set; } = new();
}
internal class PredefinedAnswerDto
{
    public string Text { get; set; }
}