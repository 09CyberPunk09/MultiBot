using Application.TextCommunication.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Threading.Tasks;
using MessageContext = Application.TextCommunication.Core.Context.MessageContext;

namespace Application.TelegramBot.Pipelines.V2.Pipelines;

[Route("/hello")]
internal class HelloCommand : ICommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder
            .Stage<WhatsUpStage>()
            .Stage<GoodByeStage>();
    }

    public Task<StageResult> Execute(MessageContext ctx)
    {
        return StageResult.TaskContentResult(new()
        {
            Text = "Hello!"
        });
    }
}

internal class WhatsUpStage : IStage
{
    public Task<StageResult> Execute(MessageContext ctx)
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

internal class WhereAreYouFromStage : IStage
{
    public Task<StageResult> Execute(MessageContext ctx)
    {
        return StageResult.TaskContentResult(new()
        {
            Text = "Where are you from?"
        });
    }
}

internal class GoodByeStage : IStage
{
    public Task<StageResult> Execute(MessageContext ctx)
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