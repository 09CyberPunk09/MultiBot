using Application.Chatting.Core.Repsonses;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

public class TomorrowAt : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Next, enter time in format HH:MM, HH:MM,..."
            },
            NextStage = typeof(TryAcceptTomorrowTime).FullName
        });
    }
}

public class TryAcceptTomorrowTime : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        try
        {
            var result = TimeParser.Parse(text);

            var time = result.FirstOrDefault();
            var now = DateTime.Now.AddDays(1);
            var date = new DateTime(now.Year, now.Month, now.Day, time.Item1, time.Item2, 0);
            var schedulerConfig = new ScheduleExpressionDto(new List<DateTime>() { date });

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