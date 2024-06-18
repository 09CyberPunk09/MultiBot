using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;

[Route("/get_tag", "📁 Get Tag Data")]
public class GettagDataCommand : ITelegramCommand
{
    public const string TAGDICTIONARY_CACHEKEY = "TagsDictionary";

    private readonly TagAppService _tagService;

    public GettagDataCommand(TagAppService tagService)
    {
        _tagService = tagService;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ReturnTagNotes>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var tags = _tagService.GetAll(ctx.User.Id);

        var b = new StringBuilder();
        b.AppendLine("Enter a number near the tag you want to open:");

        int counter = 0;
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

public class ReturnTagNotes : ITelegramStage
{
    private readonly TagAppService _tagService;

    public ReturnTagNotes(TagAppService tagService)
    {
        _tagService = tagService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(GettagDataCommand.TAGDICTIONARY_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count()))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var id = dict[number];
        var tag = _tagService.Get(id);
        var counter = 0;
        var b = new StringBuilder(Environment.NewLine);
        foreach (var item in tag.Notes)
        {
            b.AppendLine($"🔸 {++counter}. {item.Text}");
        }

        return ContentResponse.Text($"{tag.Name}: " + b.ToString());
    }
}