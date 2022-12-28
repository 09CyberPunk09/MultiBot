using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;

[Route("/timetracker_this_month_report", "This Month Report")]
public class MonthlyTrackingReportCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public MonthlyTrackingReportCommand(TimeTrackingAppService service)
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

        var dateFrom = DateTime.Now.AddMonths(-1).Date;
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

        b.AppendLine("Your Month report:");

        report = report.OrderByDescending(el => el.Date).ToList();

        b.AppendLine("🟡 Brief Report by Week:");
        //brief report by week
        report.GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                   i.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                          .Select(g =>
                          {
                              var firstDayOfWeek = g.FirstOrDefault();
                              var lastDayOfWeek = g.LastOrDefault();
                              var totalTime = new TimeSpan(g.Sum(r => r.TrackedTime.Ticks));
                              return new
                              {
                                  TotalTime = totalTime,
                                  StartOfWeek = firstDayOfWeek.Date,
                                  EndOfWeek = lastDayOfWeek.Date,
                              };
                          })
                          .ToList()
                          .ForEach(el =>
                          {
                              b.AppendLine($"🔹 {el.StartOfWeek:dd.MM.yyyy} - {el.EndOfWeek:dd.MM.yyyy} tracked {(int)el.TotalTime.TotalHours}h {el.TotalTime.Minutes}m");
                          });

        b.AppendLine();
        b.AppendLine("🔵 Brief Report by Days:");
        report.ForEach(el =>
        {
            b.AppendLine($"🔹 {Enum.GetName(el.Date.DayOfWeek)} {el.Date:dd.MM.yyyy} tracked {el.TrackedTime.Hours}h {el.TrackedTime.Minutes}m");
        });

        return ContentResponse.Text(b.ToString());
    }
}