using Application.Services;
using Autofac;
using Common.Models;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.TelegramBot.MessagePipelines.TimeTracker.Reports
{
    [Route("/timetracker_this_week_report", "This Week Report")]
    public class GetLastWeekTimeTrackingReportPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public GetLastWeekTimeTrackingReportPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
            RegisterStage(ctx =>
            {
                var activityId = GetCachedValue<Guid>(GenerateReportPipeline.SELECTEDACTIVITY_CACHEKEY,true);
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
                return new()
                {
                    Text = b.ToString()
                };
            });
        }
    }
}
