using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Settings;

[Route("/settings+menu","Settings")]
public class SettingsMenuCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Not implemented yet");
    }
}
