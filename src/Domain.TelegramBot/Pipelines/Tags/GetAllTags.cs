﻿using Application.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;

[Route("/get-tags", "📋 List Tags")]
public class GetAllTagsCommand : ITelegramCommand
{
    private readonly TagAppService _tagService;

    public GetAllTagsCommand(TagAppService tagService)
    {
        _tagService = tagService;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var tags = _tagService.GetAll(ctx.User.Id)
                                            .Where(t => !t.IsSystem);

        var b = new StringBuilder();
        b.AppendLine("All your tags:");

        var counter = 0;
        foreach (var item in tags)
        {
            ++counter;
            b.AppendLine($"🔷 {counter}. {item.Name}");
        }

        return ContentResponse.Text(b.ToString());
    }
}
