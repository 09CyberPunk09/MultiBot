using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Timetracking.TimeTracking;

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
