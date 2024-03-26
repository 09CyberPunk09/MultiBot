using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Timetracking.Reports;

[Route("/timetracker_today_report", "Today's Report")]
public class TodaysTrackingReportCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public TodaysTrackingReportCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var activityId = ctx.Cache.Get<Guid>(GenerateReportCommand.SELECTEDACTIVITY_CACHEKEY);
        List<TimeTrackingReportModel> report;

        var dateFrom = DateTime.Now.Date;
        var dateTo = DateTime.Now.AddDays(1).Date.AddSeconds(-1);

        if (activityId == Guid.Empty)
        {
            report = _service.GenerateReport(dateFrom, dateTo, null);
        }
        else
        {
            report = _service.GenerateReport(dateFrom, dateTo, activityId);
        }
        var b = new StringBuilder();

        b.AppendLine("Your Today's report:");

        report.ForEach(el =>
        {
            b.AppendLine($"🔹 {el.Date:dd.MM.yyyy} tracked {el.TrackedTime.Hours}h {el.TrackedTime.Minutes}m");
        });
        return ContentResponse.Text(b.ToString());
    }
}