using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

[Route("/every_nth_day_at")]
public class EveryNthDayCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptNumberStage>();
        builder.Stage<TryAcceptTime>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        //TODO: додати механізм вираховування періодичності відносно сьогоднішнього дня
        return ContentResponse.Text("Enter a number of days of the delay between schedule firings:");
    }
}

public class AcceptNumberStage : ITelegramStage
{
    public const string NUMBEROFDAYS_CACHEKEY = "NumberOfDays";
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if(!int.TryParse(ctx.Message.Text, out var result))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Enter a number of days of the delay between schedule firings:");
        }
        ctx.Cache.Set(NUMBEROFDAYS_CACHEKEY, result);
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
            var crons = new List<string>();
            int number = ctx.Cache.Get<int>(AcceptNumberStage.NUMBEROFDAYS_CACHEKEY, true);
            foreach (var tuple in result)
            {
                //0 0 0 1/30 * ? *
                var cron = $"0 0,{tuple.Item2} {tuple.Item1} 1/{number} * * *";
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