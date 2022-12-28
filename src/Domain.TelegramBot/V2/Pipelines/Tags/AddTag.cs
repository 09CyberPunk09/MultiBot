using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;

[Route("/new_tag", "➕ Create Tag")]
public class AddTagCommand : ITelegramCommand
{
    private readonly TagAppService _tagService;

    public AddTagCommand(TagAppService tagAppService)
    {
        _tagService = tagAppService;
    }
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<SaveTag>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter Tag name:");
    }
}
public class SaveTag : ITelegramStage
{
    private readonly TagAppService _tagService;

    public SaveTag(TagAppService tagAppService)
    {
        _tagService = tagAppService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        _tagService.Create(ctx.Message.Text, ctx.User.Id);
        return ContentResponse.Text("✅ Tag saved");
    }
}
