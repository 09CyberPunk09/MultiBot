using TelegramBot.ChatEngine;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Routing;

namespace TelegramCommunicationFramework.TestApp;


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