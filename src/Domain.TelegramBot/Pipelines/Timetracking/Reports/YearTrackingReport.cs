﻿using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;

[Route("/timetracker_this_year_report", "This Year Report")]
public class YearTrackingReportCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public YearTrackingReportCommand(TimeTrackingAppService service)
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

        int year = DateTime.Now.Year;
        var dateFrom = new DateTime(year, 1, 1);
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

        b.AppendLine("🟡 Report by Months:");
        report.GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetMonth(i.Date))
                          .Select(g =>
                          {
                              var firstDayOfWeek = g.FirstOrDefault();
                              var lastDayOfWeek = g.LastOrDefault();
                              var totalTime = new TimeSpan(g.Sum(r => r.TrackedTime.Ticks));
                              return new
                              {
                                  TotalTime = totalTime,
                                  StartOfMonth = firstDayOfWeek.Date
                              };
                          })
                          .ToList()
                          .ForEach(el =>
                          {
                              b.AppendLine($"🔹 {el.StartOfMonth.ToString("MMM", CultureInfo.InvariantCulture)}: {(int)el.TotalTime.TotalHours}h {el.TotalTime.Minutes}m");
                          });

        b.AppendLine();

        return ContentResponse.Text(b.ToString());
    }
}
