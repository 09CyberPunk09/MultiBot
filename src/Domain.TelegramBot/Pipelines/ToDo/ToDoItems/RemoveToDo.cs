using Application.Services;
using Application.TelegramBot.Commands.Pipelines.ToDo.ToDoItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;
enum Choice
{
    Remove,
    MarkAsDone
}

[Route("/remove_todo", "🗑 Remove")]
public class RemoveToDoCommand : ITelegramCommand
{
    public const string SELECTEDITEMID_CACHEKEY = "Selected Item Number To Delete";

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AskAboutAction>();
        builder.Stage<AcceptandDelete>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter a number near a ToDo item which you want to delete:");
    }
}
public class AskAboutAction : ITelegramStage
{
    private readonly RoutingTable _routingTable;
    public AskAboutAction(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var numbers = ctx.Cache.Get<Dictionary<int, Guid>>(GetToDoListCommand.TODOSORDER_CACHEKEY);
        if (!int.TryParse(ctx.Message.Text, out var t) ||
            !numbers.TryGetValue(t, out var noteId))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text($"You must to enter a number which is in the range of todo items. If the list disaperared, enter {_routingTable.AlternativeRoute<GetToDoListCommand>()}.");
        }
        ctx.Cache.Set(RemoveToDoCommand.SELECTEDITEMID_CACHEKEY, noteId);
        return ContentResponse.New(new()
        {
            Text = "Noew,Select in what way you want to remove ToDo item from the list:",
            Menu = new(MenuType.MessageMenu, new[]
            {
                new[]{ new Button("✅Mark as Done",((int)Choice.MarkAsDone).ToString()) },
                new[]{ new Button("❌Remove",((int)Choice.Remove).ToString()) }
            })
        });
    }
}
public class AcceptandDelete : ITelegramStage
{
    private readonly ToDoAppService _todoService;
    public AcceptandDelete(ToDoAppService toDoAppService)
    {
        _todoService = toDoAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (!Enum.TryParse<Choice>(ctx.Message.Text, out var ch))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Please, select a value from the menu");
        }

        var todoItemId = ctx.Cache.Get<Guid>(RemoveToDoCommand.SELECTEDITEMID_CACHEKEY);
        var todoItem = _todoService.GetToDoItem(todoItemId);
        if (ch == Choice.MarkAsDone)
        {
            todoItem.IsDone = true;
            _todoService.UpdateToDoItem(todoItem);
        }
        else
        {
            _todoService.DeleteToDoItem(todoItem.Id);
        }
        return ContentResponse.Text("✅ Done");
    }
}
