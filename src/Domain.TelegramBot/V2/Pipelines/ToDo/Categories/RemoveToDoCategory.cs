using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;

[Route("/delete_todo_category", "❌ Remove Category")]
public class RemoveToDoCategoryCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ConfirmSelectedItem>();
        builder.Stage<RemoveCategorey>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Now, enter a number near your desicion to delete the activity");
    }
}

public class ConfirmSelectedItem : ITelegramStage
{
    public const string CATEGORY_ID_CACHEKEY = "CategoryIdToRemove";

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(CategoriesMenuCommand.TODO_CATEGORIES_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var id = dict[number];
        ctx.Cache.Set(CATEGORY_ID_CACHEKEY, id.ToString());

        return ContentResponse.New(new()
        {
            Text = "Are you sure you want to delete?",
            Menu = new(MenuType.MessageMenu, new[]
            {
                new[] 
                { 
                    new Button("🟩 Yes",true.ToString()),
                    new Button("🟥 No",false.ToString())
                }
            })
        });
    }
}

public class RemoveCategorey : ITelegramStage
{
    private readonly ToDoAppService _service;
    public RemoveCategorey(ToDoAppService service)
    {
        _service = service;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (bool.TryParse(ctx.Message.Text, out var result))
        {
            if (result)
            {
                _service.DeleteToDoCategory(ctx.Cache.Get<Guid>(ConfirmSelectedItem.CATEGORY_ID_CACHEKEY));
                return ContentResponse.Text("✅ Done");
            }
        }
        return ContentResponse.Text("Canceled");
    }
}
