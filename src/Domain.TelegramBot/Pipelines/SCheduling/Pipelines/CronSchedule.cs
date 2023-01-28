using Application.Chatting.Core.Repsonses;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Common.Entites.Scheduling;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

//[Route("/cron_schedule")]
public class CronScheduleCommand : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Enter a cron. You can use https://www.freeformatter.com/cron-expression-generator-quartz.html for building a cron schedule"
            },
            NextStage = typeof(AcceptCronStage).FullName
        });
    }
}

public class AcceptCronStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (CronExpression.IsValidExpression(ctx.Message.Text))
        {
            var schedulerConfig = new ScheduleExpressionDto(new List<string>() { ctx.Message.Text });
            schedulerConfig.Description = ctx.Message.Text;
            ctx.Cache.Set(ScheduleExpressionDto.CACHEKEY, schedulerConfig);

            ctx.Response.InvokeNextImmediately = true;
            return Task.FromResult(new StageResult() { });
        }
        else
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Enter a valid cron");
        }
    }
}
