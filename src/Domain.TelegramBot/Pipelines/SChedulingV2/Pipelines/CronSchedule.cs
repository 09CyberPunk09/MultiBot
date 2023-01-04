using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

[Route("/cron_schedule")]
public class CronScheduleCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptCronStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter a cron. You can use https://www.freeformatter.com/cron-expression-generator-quartz.html for building a cron schedule");
    }
}

public class AcceptCronStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (CronExpression.IsValidExpression(ctx.Message.Text))
        {
            var schedulerConfig = new ScheduleExpressionDto(new List<string>() { ctx.Message.Text });

            ctx.Cache.Set(ScheduleExpressionDto.CACHEKEY, schedulerConfig);
            return ContentResponse.Text("Scheduler configured");
        }
        else
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Enter a valid cron");
        }
    }
}
