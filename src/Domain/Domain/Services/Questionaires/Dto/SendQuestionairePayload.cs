using Common;
using Common.Entites.Scheduling;
using System;

namespace Application.Services.Questionaires.Dto;

public class SendQuestionaireJobPayload : JobConfigurationPayload
{
    public string QuestionaireName { get; set; }
    public Guid QuestionaireId { get; init; }
    public ScheduleExpressionDto ScheduleExpression { get; set; }
    public long[] TelegramChatIds { get; set; }
}
