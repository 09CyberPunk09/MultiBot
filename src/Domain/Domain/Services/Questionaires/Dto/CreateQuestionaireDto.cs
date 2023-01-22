using Common.Entites.Questionaires;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;

namespace Application.Services.Questionaires.Dto;

public class CreateQuestionaireDto
{
    public string Text { get; init; }
    public Guid UserId { get; init; }
    public ScheduleExpressionDto SchedulerExpression { get; init; }
}

public class CreateQuestionDto
{
    public string Text { get; set; }
    public AnswerType AnswerType { get; set; }
    public int? RangeMin { get; set; }
    public int? RangeMax { get; set; }
    public List<CreatePredefinedAnswerDto> PredefinedAnswers { get; set; }
}

public class CreatePredefinedAnswerDto
{
    public string Text { get; set; }
}