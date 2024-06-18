using Application.TelegramBot.Commands.Pipelines.ToDo.ToDoItems;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Commands.Pipelines.ToDo;

[Route("/todo_info", "📋 ToDo List")]
public class ToDoMenuCommand : ITelegramCommand
{
    private readonly RoutingTable _routingTable;
    public ToDoMenuCommand(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "ToDo menu",
            Menu = new(MenuType.MenuKeyboard, new[]
            {
                new[] { new Button(_routingTable.AlternativeRoute<CreateToDoCommand>()) },
                new[] { new Button(_routingTable.AlternativeRoute<CategoriesMenuCommand>()) },
                new[] { new Button(_routingTable.AlternativeRoute<GetToDoListCommand>()) },
            })
        });
    }
}
