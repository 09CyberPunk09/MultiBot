using Application.Services;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;

[Route("/timetracker_yesterday_report", "Get Yesterday's Report")]
public class YesterdayTrackingReportCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public YesterdayTrackingReportCommand(TimeTrackingAppService service)
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

        var dateFrom = DateTime.Now.AddDays(-1).Date;
        var dateTo = DateTime.Now.Date.AddSeconds(-1);

        if (activityId == Guid.Empty)
        {
            report = _service.GenerateReport(dateFrom, dateTo, null);
        }
        else
        {
            report = _service.GenerateReport(dateFrom, dateTo, activityId);
        }
        var b = new StringBuilder();

        b.AppendLine("Your Yesterday's report:");

        report.ForEach(el =>
        {
            b.AppendLine($"🔹 {el.Date:dd.MM.yyyy} tracked {el.TrackedTime.Hours}h {el.TrackedTime.Minutes}m");
        });
        return ContentResponse.Text(b.ToString());
    }
}
