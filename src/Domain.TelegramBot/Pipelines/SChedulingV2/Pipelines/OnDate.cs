using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

[Route("/on_date_at_time_schedule")]
public class OnDateCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptDateStage>();
        builder.Stage<AcceptTimeAndSave>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter date int format mm.dd.yyyy which is the future");
    }
}

public class AcceptDateStage : ITelegramStage
{
    public const string SELECTEDDATE_CACHEKEY = "SelectedDate";
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if(!DateTime.TryParse(ctx.Message.Text,out var result))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text($"Please, enter a valid value");
        }
        ctx.Cache.Set(SELECTEDDATE_CACHEKEY, result);
        return ContentResponse.Text("Enter time in format HH:MM, HH:MM,...");
    }
}

public class AcceptTimeAndSave : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        try
        {
            var result = TimeParser.Parse(text);
            var date = ctx.Cache.Get<DateTime>(AcceptDateStage.SELECTEDDATE_CACHEKEY, true);
            List<DateTime> datesResult = new();
            foreach (var time in result)
            {
                datesResult.Add(date.AddHours(time.Item1).AddMinutes(time.Item2));
            }
            var schedulerConfig = new ScheduleExpressionDto(datesResult);

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