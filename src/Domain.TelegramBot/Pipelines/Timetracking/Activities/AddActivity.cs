﻿using Application.Services;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking.Activities;

[Route("/add_activity")]
public class AddActivityCommand : ITelegramCommand
{
    private readonly TimeTrackingAppService _service;

    public AddActivityCommand(TimeTrackingAppService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<SaveActivityStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enater new activity name:");
    }
}

public class SaveActivityStage : ITelegramStage
{
    private readonly TimeTrackingAppService _service;

    public SaveActivityStage(TimeTrackingAppService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        _service.CreateTimeTrackingActivity(ctx.Message.Text, ctx.User.Id);
        return ContentResponse.Text("✅ Done");
    }
}
