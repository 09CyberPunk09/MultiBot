using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
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
