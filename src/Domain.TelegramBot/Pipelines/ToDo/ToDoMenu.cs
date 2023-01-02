using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.ToDo.ToDoItems;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.Categories;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;
using System;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

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
