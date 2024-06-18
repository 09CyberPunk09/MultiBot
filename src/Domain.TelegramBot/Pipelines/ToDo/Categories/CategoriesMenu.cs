using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;

[Route("/manage_todo_categories", "Manage ToDo Categories")]
public class CategoriesMenuCommand : ITelegramCommand
{
    private readonly ToDoAppService _todoService;
    private readonly RoutingTable _routingTable;
    public const string TODO_CATEGORIES_CACHEKEY = "TODO_CATEGORIES_CACHEKEY";
    public CategoriesMenuCommand(ToDoAppService todoService, RoutingTable routingTable)
    {
        _todoService = todoService;
        _routingTable = routingTable;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var userId = ctx.User.Id;
        var categories = _todoService.GetAllCategories(userId);
        var sb = new StringBuilder();
        sb.AppendLine("Your TODO items: ");
        int counter = 0;
        var orders = new Dictionary<int, Guid>();
        if (categories.Any())
        {
            categories.ToList()
            .ForEach(c =>
            {
                sb.AppendLine($"🔸 {++counter}.{c.Name}");
                orders.Add(counter, c.Id);
            });
        }
        else
        {
            sb.AppendLine("No ToDos yet");
        }
        ctx.Cache.Set(TODO_CATEGORIES_CACHEKEY, orders);
        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = new(MenuType.MenuKeyboard, new[]
        {
                new[]{ new Button(_routingTable.AlternativeRoute<CreateToDoCategoryCommand>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<RemoveToDoCategoryCommand>()) },
        })
        });
    }
}