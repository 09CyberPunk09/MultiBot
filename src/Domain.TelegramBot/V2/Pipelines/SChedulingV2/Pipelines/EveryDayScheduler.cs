using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.SChedulingV2.Pipelines;

[Route("/every_day_at_schedule")]
public class EveryDaySchedulerCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<TryAcceptTime>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter time in format HH:MM, HH:MM,...");
    }
}

public class TryAcceptTime : ITelegramStage
{
public Task<StageResult> Execute(TelegramMessageContext ctx)
{
    var text = ctx.Message.Text;
    try
    {
        var result = TimeParser.Parse(text);
        //0 0,35 7 ? * * *
        //ScheduleExpressionDto
        var crons = new List<string>();
        foreach (var tuple in result)
        {
            var cron = $"0 0,{tuple.Item2} {tuple.Item1} ? * * *";
            crons.Add(cron);
        }
        var schedulerConfig = new ScheduleExpressionDto(crons);

        ctx.Cache.Set(ScheduleExpressionDto.CACHEKEY, schedulerConfig);

        return ContentResponse.Text("Schedule configured");
    }
    catch (ArgumentException)
    {
        ctx.Response.ForbidNextStageInvokation();
        return ContentResponse.Text("You entered a not valid value. Try entering a string in format HH:MM, HH:MM,...");
    }

}
}