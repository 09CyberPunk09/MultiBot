using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;

[Route("/timetracker_this_week_report", "This Week Report")]
public class WeekTrackingReportCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public WeekTrackingReportCommand(TimeTrackingAppService service)
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

        var dateFrom = DateTime.Now.AddDays(-7).Date;
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

        b.AppendLine("Your Week report:");

        report = report.OrderByDescending(el => el.Date).ToList();

        report.ForEach(el =>
        {
            b.AppendLine($"🔹 {Enum.GetName(el.Date.DayOfWeek)} {el.Date:dd.MM.yyyy} tracked {el.TrackedTime.Hours}h {el.TrackedTime.Minutes}m");
        });
        return ContentResponse.Text(b.ToString());
    }
}
