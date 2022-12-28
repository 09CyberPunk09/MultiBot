using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Activities;

[Route("/remove_activity")]
//todo: make an analyse where we can generalize the element selection
public class RemoveActivityCommand : ITelegramCommand
{
    public const string ID_CACHEKEY = "ActivityIdToRemove";
    public const string ACTIVITIES_CACHEKEY = "ActivitiesDicitonary";
    private readonly TimeTrackingAppService _service;

    public RemoveActivityCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ConfirmRemoveActivity>();
        builder.Stage<RemoveActivity>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var activities = _service.GetAllActivities(ctx.User.Id);
        var b = new StringBuilder();
        b.AppendLine("Activities: ");
        var activitiesDict = new Dictionary<int, Guid>();
        int a = 1;

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
                    Text = "Now, enter a number near your desicion to delete the activity"
                }
            }
        });
    }
}

public class ConfirmRemoveActivity : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(RemoveActivityCommand.ACTIVITIES_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var id = dict[number];
        ctx.Cache.Set(RemoveActivityCommand.ID_CACHEKEY, id.ToString());

        return ContentResponse.New(new()
        {
            Text = "Are you sure you want to delete?",
            Menu = new(MenuType.MessageMenu, new[]
            {
                new[]
                {
                    new Button("🟩 Yes",true.ToString()),
                    new Button("🟥 No",false.ToString()),
                }
            })
        });
    }
}

public class RemoveActivity : ITelegramStage
{
    private readonly TimeTrackingAppService _service;

    public RemoveActivity(TimeTrackingAppService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (bool.TryParse(ctx.Message.Text, out var result))
        {
            if (result)
            {
                _service.RemoveActivity(Guid.Parse(ctx.Cache.Get<string>(RemoveActivityCommand.ID_CACHEKEY)));
                return ContentResponse.Text("✅ Done");
            }
        }
        return ContentResponse.Text("Canceled");
    }
}