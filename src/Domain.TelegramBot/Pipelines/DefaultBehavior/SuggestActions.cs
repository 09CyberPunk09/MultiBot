using Application.TelegramBot.Commands.Pipelines.Reminders;
using Application.TelegramBot.Pipelines.V2.Pipelines.NotesV2;
using Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.DefaultBehavior;

[Route("/suggest_actions")]
public class SuggestActionsCommand : ITelegramCommand
{
    public const string TEXTCONTENT_CACHEKEY = "EnteredText";
    private readonly RoutingTable _rt;
    public SuggestActionsCommand(RoutingTable routingTable)
    {
        _rt = routingTable;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        ctx.Cache.Set(TEXTCONTENT_CACHEKEY,ctx.Message.Text);
        return ContentResponse.New(new()
        {
            Text = "I didnt recognized your message as any command.You can save your text as one of suggested entities:",
            Menu = new Menu(Menu.MenuType.MessageMenu,
            new[]
            {
                new[] 
                { 
                    new Button(_rt.AlternativeRoute<AddNoteCommand>(), _rt.Route<AddNoteCommand>()),
                    new Button(_rt.AlternativeRoute<CreateReminderCommand>(), _rt.Route<CreateReminderCommand>()) 
                },
                new[] 
                { 
                    new Button(_rt.AlternativeRoute<CreateToDoCommand>(), _rt.Route<CreateToDoCommand>()) 
                }
            }
            )
        });
    }
}
