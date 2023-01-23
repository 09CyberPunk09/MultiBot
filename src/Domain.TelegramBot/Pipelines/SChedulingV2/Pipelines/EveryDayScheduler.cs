using Application.Chatting.Core.Repsonses;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.SChedulingV2.Pipelines;

//[Route("/every_day_at_schedule")]
public class EveryDaySchedulerCommand : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Next, enter time in format HH:MM, HH:MM,..."
            },
            NextStage = typeof(TryAcceptTime).FullName
        });
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

            ctx.Response.InvokeNextImmediately = true;
            return Task.FromResult(new StageResult() { });
        }
        catch (ArgumentException)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("You entered a not valid value. Try entering a string in format HH:MM, HH:MM,...");
        }

    }
}