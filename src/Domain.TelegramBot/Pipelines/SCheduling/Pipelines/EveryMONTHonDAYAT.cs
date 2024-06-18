using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

//[Route("/every_month_on_schedule")]
public class EveryMONTHonDAYATCommand : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Enter a number of a day on which the schedule will be triggered every month"
            },
            NextStage = typeof(AcceptDayStage).FullName
        });
    }
}

public class AcceptDayStage : ITelegramStage
{
    public const string NUMBER_CACHEKEY = "DayOfMonth";
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (!int.TryParse(ctx.Message.Text, out var result))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Enter a number of a day on which the schedule will be triggered every month");
        }
        ctx.Cache.Set(NUMBER_CACHEKEY, result);

        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Enter time in format HH:MM, HH:MM,..."
            },
            NextStage = typeof(TryAcceptTimeAndSave).FullName
        });
    }
}

public class TryAcceptTimeAndSave : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        try
        {
            var result = TimeParser.Parse(text);

            var crons = new List<string>();
            var number = ctx.Cache.Get<int>(AcceptDayStage.NUMBER_CACHEKEY, true);
            StringBuilder selectedTimes = new();
            foreach (var tuple in result)
            {
                var cron = $"0 0,{tuple.Item2} {tuple.Item1} 1/{number} * * *";
                selectedTimes.Append($" {tuple.Item1}:{tuple.Item2}");

                crons.Add(cron);
            }
            var schedulerConfig = new ScheduleExpressionDto(crons);
            schedulerConfig.Description = $"Every month on {number}th at {selectedTimes}";
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