using Application.TelegramBot.Commands.Pipelines.Account;
using Application.TelegramBot.Commands.Pipelines.Questionaires;
using Application.TelegramBot.Commands.Pipelines.Reminders;
using Application.TelegramBot.Commands.Pipelines.Settings;
using Application.TelegramBot.Commands.Pipelines.ToDo;
using Application.TelegramBot.Pipelines.V2.Pipelines.NotesV2;
using Application.TelegramBot.Pipelines.V2.Pipelines.Timetracking;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.MainMenu;

[Route("/menu", "🏠 Home")]
public class MenuCommand : ITelegramCommand
{
    private readonly RoutingTable rt;
    public MenuCommand(RoutingTable routingTable)
    {
        rt = routingTable;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "Main menu",
            Menu = new Menu(Menu.MenuType.MenuKeyboard,
            new[]
            {
                new[]
                {
                    new Button(rt.AlternativeRoute<NotesMenuCommand>()),
                    new Button(rt.AlternativeRoute<TimeTrackingInfoCommand>())
                },
                new[]
                {
                    new Button(rt.AlternativeRoute<QuestionairesMenu>()),
                    new Button(rt.AlternativeRoute<RemindersMenuCommand>()),
                    new Button(rt.AlternativeRoute<ToDoMenuCommand>())
                },
                new[]
                {
                    new Button(rt.AlternativeRoute<AccountMenuCommand>()),
                    new Button(rt.AlternativeRoute<SettingsMenuCommand>())
                }
            })
        });
    }
}
