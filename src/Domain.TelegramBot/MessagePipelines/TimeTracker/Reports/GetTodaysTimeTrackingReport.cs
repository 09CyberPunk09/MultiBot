using Application.Services;
using Autofac;
using Common.Models;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/timetracker_today_report","Today's Report")]
    public class GetTodaysTimeTrackingReport : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public GetTodaysTimeTrackingReport(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
            RegisterStage(() =>
            {
                var activityId = GetCachedValue<Guid>(GenerateReportPipeline.SELECTEDACTIVITY_CACHEKEY,true);
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
                return new()
                {
                    Text = b.ToString()
                };
            });
        }
    }
}
