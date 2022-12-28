using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Timetracking.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;

[Route("/timetracker_report", "📊 Time Tracking Report")]
public class GenerateReportCommand : ITelegramCommand
{
    public const string ACTIVITIES_CACHEKEY = "ActivitiesOfUser";
    public const string SELECTEDACTIVITY_CACHEKEY = "SelectedActivity";
    private readonly TimeTrackingAppService _service;
    public GenerateReportCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptSelectedActivity>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var activities = _service.GetAllActivities(ctx.User.Id);
        var b = new StringBuilder();
        b.AppendLine("Activities: ");
        var activitiesDict = new Dictionary<int, Guid>();
        int a = 1;

        activitiesDict[0] = Guid.Empty;
        b.AppendLine($"🔸 {0}. All");

        activities.ForEach(activity =>
        {
            activitiesDict[a] = activity.Id;

            b.AppendLine($"🔸 {a}. {activity.Name}");

            a++;
        });

        ctx.Cache.Set(ACTIVITIES_CACHEKEY, activitiesDict);

        return ContentResponse.New(new()
        {
            Text = b.ToString(),
            MultiMessages = new()
            {
                new()
                {
                    Text = "Now, enter a number near your desicion"
                }
            }
        });
    }
}
public class AcceptSelectedActivity : ITelegramStage
{
    private readonly TimeTrackingAppService _service;
    private readonly RoutingTable _routingTable;

    public AcceptSelectedActivity(TimeTrackingAppService service, RoutingTable routingTable)
    {
        _service = service;
        _routingTable = routingTable;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(GenerateReportCommand.ACTIVITIES_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count()))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var activityId = dict[number];
        ctx.Cache.Set(GenerateReportCommand.SELECTEDACTIVITY_CACHEKEY, activityId);

        string activityText;
        if (activityId == Guid.Empty)
        {
            activityText = "All";
        }
        else
        {
            var activity = _service.GetActivity(activityId);
            activityText = activity.Name;
        }

        return ContentResponse.New(new()
        {
            Text = $"Your selected Activity - \"{activityText}\". Select a report type:",
            Menu = new(MenuType.MessageMenu, new[]
            {
                // todo: fix reporting
                new[] { new Button("Today",      _routingTable.AlternativeRoute<TodaysTrackingReportCommand>()) },
                new[] { new Button("Yesterday",  _routingTable.AlternativeRoute<YesterdayTrackingReportCommand>()) },
                new[] { new Button("This Week",  _routingTable.AlternativeRoute<WeekTrackingReportCommand>()) },
                new[] { new Button("Last Month", _routingTable.AlternativeRoute<MonthlyTrackingReportCommand>()) },
                new[] { new Button("This Year", _routingTable.AlternativeRoute<YearTrackingReportCommand>()) },
                //todo: implement it. for now i do not have an idea how to do this
                //ButtonRow("Specific","meow")
            })
        });
    }
}