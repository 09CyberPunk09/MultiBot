using Application.Chatting.Core.Repsonses;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

//[Route("/on_date_at_time_schedule")]
public class OnDateCommand : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Enter date int format mm.dd.yyyy or mm.dd which is in the future"
            },
            NextStage = typeof(AcceptDateStage).FullName
        });

    }
}

public class AcceptDateStage : ITelegramStage
{
    public const string SELECTEDDATE_CACHEKEY = "SelectedDate";
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var onlyDaysAndMonths = Regex.IsMatch(ctx.Message.Text, "^(0[1-9]|1[0-2]).(0[1-9]|1\\d|2\\d|3[01])$");
        if (!DateTime.TryParse(ctx.Message.Text, out var result) || onlyDaysAndMonths)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text($"Please, enter a valid value");
        }

        if (onlyDaysAndMonths)
        {
            ctx.Cache.Set(SELECTEDDATE_CACHEKEY, DateTime.Parse($"{ctx.Message.Text}.{DateTime.Now.Year}"));
        }
        else
        {
            ctx.Cache.Set(SELECTEDDATE_CACHEKEY, result);
        }
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Enter time in format HH:MM, HH:MM,..."
            },
            NextStage = typeof(AcceptTimeAndSave).FullName
        });
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