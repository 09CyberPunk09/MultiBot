using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using Autofac;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.TimeTracking;

[Route("/track_out", "📥 Track out")]
//TODO: Add ability to select time where the track was out
public class TrackOutCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;
    public TrackOutCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var user = ctx.User;
        var lastActivity = _service.GetLastTrackedActivity(user.Id);
        _service.TrackOut(lastActivity.Id, DateTime.Now);
        return ContentResponse.Text($"✅ Stopped tracking at ⏱{DateTime.Now:HH:mm dd:MM:yyyy}");
    }
}
