using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Text;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Activities;

[Route("/activities", "🗄 Activities")]
public class ActivityManagementCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    private readonly RoutingTable _routingTable;
    public ActivityManagementCommand(
        TimeTrackingAppService service,
        RoutingTable routingTable
        )
    {
        _service = service;
        _routingTable = routingTable;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var activities = _service.GetAllActivities(ctx.User.Id);
        var b = new StringBuilder();
        b.AppendLine("Activities: ");
        int a = 1;

        activities.ForEach(activity =>
        {
            b.AppendLine($"🔸 {a}. {activity.Name}");
            a++;
        });

        return ContentResponse.New(new()
        {
            Text = b.ToString(),
            Menu = new(MenuType.MenuKeyboard, new[]
            {
                //todo: change to use getroute
                new[]{  new Button("Add","/add_activity") },
                new[]{ new Button("Remove","/remove_activity") }
            }),
        });
    }
}
