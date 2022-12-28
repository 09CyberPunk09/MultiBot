using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines;

[Route("/hello")]
internal class HelloCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder
            .Stage<WhatsUpStage>()
            .Stage<GoodByeStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return StageResult.TaskContentResult(new()
        {
            Text = "Hello!"
        });
    }
}

internal class WhatsUpStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "What's up?",
            },
            NextStage = typeof(WhereAreYouFromStage).FullName
        });
    }
}

internal class WhereAreYouFromStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return StageResult.TaskContentResult(new()
        {
            Text = "Where are you from?"
        });
    }
}

internal class GoodByeStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Goodbye!",
            },
            NextStage = typeof(WhereAreYouFromStage).FullName
        });
    }
}