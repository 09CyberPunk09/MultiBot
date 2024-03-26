using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.TimeTracking;
[Route("/track_in", "📥 Track In")]
public class TrackInCommand : ITelegramCommand
{
    public const string ACTIVITIES_CACHEKEY = "activitiesDictionary";
    private readonly TimeTrackingAppService _service;
    public TrackInCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptTrackIn>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var data = _service.GetAllActivities(ctx.User.Id);
        if (data == null || data.Count == 0)
        {
            var activity = _service.CreateTimeTrackingActivity("Default", ctx.User.Id);
            data = new() { activity };
        }


        var b = new StringBuilder();
        b.AppendLine("Enter a number near the Activity you want to track in:");

        var counter = 0;
        var dictionary = new Dictionary<int, Guid>();

        foreach (var item in data)
        {
            ++counter;
            b.AppendLine($"🔸 {counter}. {item.Name}");
            dictionary[counter] = item.Id;
        }

        ctx.Cache.Set(ACTIVITIES_CACHEKEY, dictionary);

        return ContentResponse.Text(b.ToString());
    }
}

public class AcceptTrackIn : ITelegramStage
{
    private readonly TimeTrackingAppService _service;
    public AcceptTrackIn(TimeTrackingAppService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(TrackInCommand.ACTIVITIES_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var id = dict[number];
        _service.TrackIn(id, DateTime.Now, ctx.User.Id);

        //todo: make track in possible only if the previous entries are complete
        //todo: make track out possible only if the previous entries are not complete
        return ContentResponse.Text($"✅Done. Started tracking at ⏱{DateTime.Now:HH:mm dd:MM:yyyy}");
    }
}
