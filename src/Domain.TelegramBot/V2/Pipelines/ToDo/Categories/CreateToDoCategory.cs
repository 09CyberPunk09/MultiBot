using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;

[Route("/new_todo_category", "➕ Create Category")]
public class CreateToDoCategoryCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptCategoryNameAndSave>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter new ToDo category name:");
    }
}
public class AcceptCategoryNameAndSave : ITelegramStage
{
    private readonly ToDoAppService _todoService;
    public AcceptCategoryNameAndSave(ToDoAppService todoService)
    {
        _todoService = todoService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        _todoService.CreateCategory(ctx.User.Id, ctx.Message.Text);
        return ContentResponse.Text("✅ Done");
    }
}