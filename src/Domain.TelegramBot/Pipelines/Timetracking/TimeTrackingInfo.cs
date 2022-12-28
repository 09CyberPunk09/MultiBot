using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Timetracking.TimeTracking;
using Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Activities;
using Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Reports;
using Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.TimeTracking;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking;

[Route("/time_tracker", "⏱ Time tracker")]
public class TimeTrackingInfoCommand : ITelegramCommand
{
    private readonly RoutingTable _routingTable;
    public TimeTrackingInfoCommand(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = @"Welcome to time tracker. You can user few commands to efectively manage your time. Here they are:
/track_in for start tracking time for specific activity
/track_out for stop tracking for last selected activity
/activities for managing activities
Or, use the menu below:",
            Menu = new(MenuType.MenuKeyboard, new[]
            {
               new[]
               {
                   new Button(_routingTable.AlternativeRoute<TrackInCommand>()),
                   new Button(_routingTable.AlternativeRoute<TrackOutCommand>())
               },
               new[]
               {
                   new Button(_routingTable.AlternativeRoute<ActivityManagementCommand>())
               },
               new[]
               {
                   new Button(_routingTable.AlternativeRoute<GenerateReportCommand>())
               }
            })
        });
    }
}
