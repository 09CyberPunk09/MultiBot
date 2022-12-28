using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo;

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
        throw new NotImplementedException();
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
