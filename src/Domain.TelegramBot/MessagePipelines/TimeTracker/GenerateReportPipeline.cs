using Application.Services;
using Autofac;
using Domain.TelegramBot.MessagePipelines.TimeTracker.Reports;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/timetracker_report", "📊 Time Tracking Report")]
    public class GenerateReportPipeline : MessagePipelineBase
    {
        public const string ACTIVITIES_CACHEKEY = "ActivitiesOfUser";
        public const string SELECTEDACTIVITY_CACHEKEY = "SelectedActivity";
        private readonly TimeTrackingAppService _service;

        public GenerateReportPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();

            RegisterStage(AskActivityNumber);
            RegisterStage(AcceptSelectedActivity);
        }

        public ContentResult AskActivityNumber(MessageContext ctx)
        {
            var activities = _service.GetAllActivities(GetCurrentUser().Id);
            var b = new StringBuilder();
            b.AppendLine("Activities: ");
            var activitiesDict = new Dictionary<int, Guid>();
            int a = 1;

            activitiesDict[0] = Guid.Empty;
            b.AppendLine($"{0}. All");

            activities.ForEach(activity =>
            {
                activitiesDict[a] = activity.Id;

                b.AppendLine($"{a}. {activity.Name}");

                a++;
            });

            SetCachedValue(ACTIVITIES_CACHEKEY, activitiesDict, ctx.Recipient);

            return new()
            {
                Text = b.ToString(),
                MultiMessages = new()
                {
                    new()
                    {
                        Text = "Now, enter a number near your desicion"
                    }
                }
            };
        }

        public ContentResult AcceptSelectedActivity(MessageContext ctx)
        {
            var dict = GetCachedValue<Dictionary<int,Guid>>(ACTIVITIES_CACHEKEY);
            if(!(int.TryParse(ctx.Message.Text,out var number) && (number >= 0 && number <= dict.Count())))
            {
                ForbidMovingNext();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var activityId = dict[number];
            SetCachedValue(SELECTEDACTIVITY_CACHEKEY, activityId);

            string activityText = "";
            if(activityId == Guid.Empty)
            {
                activityText = "All";
            }
            else
            {
                var activity = _service.GetActivity(activityId);
                activityText = activity.Name;
            }

            return new()
            {
                Text = $"Your selected Activity - \"{activityText}\". Select a report type:",
                Buttons = new InlineKeyboardMarkup(new[]
                {
                    ButtonRow("Today",GetRoute<GetTodaysTimeTrackingReport>().Route),
                    ButtonRow("Yesterday",GetRoute<GetYesterdaysTimeTrackingReport>().Route),
                    ButtonRow("This Week",GetRoute<GetLastWeekTimeTrackingReportPipeline>().Route),
                    ButtonRow("Last Month",GetRoute<GetThisMonthTimeTrackingReportPipeline>().Route),
                    ButtonRow("This Year",GetRoute<GenerateYearTimeTrackingReportPipeline>().Route),
                    //todo: implement it. for now i do not have an idea how to do this
                    //ButtonRow("Specific","meow")
                })
            };
        }

  
    }
}
