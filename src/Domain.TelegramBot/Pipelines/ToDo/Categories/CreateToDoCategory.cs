using Application.Services;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

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