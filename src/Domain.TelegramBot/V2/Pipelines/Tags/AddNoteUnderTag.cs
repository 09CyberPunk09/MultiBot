using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;

[Route("/new_tagged_note", "➕ New Tagged Note")]
public class AddNoteUnderTagCommand : ITelegramCommand
{
    public const string TAGDICTIONARY_CACHEKEY = "TagDictionary";
    private readonly TagAppService _tagService;

    public AddNoteUnderTagCommand(TagAppService tagAppService)
    {
        _tagService = tagAppService;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AskForNoteTextStage>();
        builder.Stage<SaveNoteUnderTag>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var tags = _tagService.GetAll(ctx.User.Id);

        var b = new StringBuilder();
        b.AppendLine("Enter a number near the tag you want to open:");

        var counter = 0;
        var dictionary = new Dictionary<int, Guid>();

        foreach (var item in tags)
        {
            ++counter;
            b.AppendLine($"🔷 {counter}. {item.Name}");
            dictionary[counter] = item.Id;
        }

        ctx.Cache.Set(TAGDICTIONARY_CACHEKEY, dictionary);

        return ContentResponse.Text(b.ToString());
    }
}
public class AskForNoteTextStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(AddNoteUnderTagCommand.TAGDICTIONARY_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }
        var id = dict[number];

        ctx.Cache.Set("AddSetItemSetId", id);
        return ContentResponse.Text("Note text:");
    }
}
public class SaveNoteUnderTag : ITelegramStage
{
    private readonly TagAppService _tagService;

    public SaveNoteUnderTag(TagAppService tagAppService)
    {
        _tagService = tagAppService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var tagId = ctx.Cache.Get<Guid>("AddSetItemSetId");
        _tagService.CreateNoteUnderTag(tagId, ctx.Message.Text, ctx.User.Id);
        return ContentResponse.Text("✅ Note saved");
    }
}
