using Application.Services;
using Autofac;
using Common.Models;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.TimeTracker.Reports
{
    [Route("/timetracker_this_year_report", "This Year Report")]
    public class GenerateYearTimeTrackingReportPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public GenerateYearTimeTrackingReportPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
            RegisterStage(() =>
            {
                var activityId = GetCachedValue<Guid>(GenerateReportPipeline.SELECTEDACTIVITY_CACHEKEY, true);
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

                return new()
                {
                    Text = b.ToString()
                };
            });
        }
    }
}
