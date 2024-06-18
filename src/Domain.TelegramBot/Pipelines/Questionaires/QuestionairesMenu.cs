using Application.TelegramBot.Commands.Pipelines.Questionaires.Options;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires;


[Route("/questionaires", "Questionaires")]
public class QuestionairesMenu : ITelegramCommand
{
    private readonly RoutingTable _routingTable;
    public QuestionairesMenu(RoutingTable routingTable)
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
            Text = "Questionaires Menu",
            Menu = new Menu(Menu.MenuType.MenuKeyboard, new[]
            {
                new[]
                {
                    new Button(_routingTable.AlternativeRoute<CreateQuestionaireCommand>()),
                    new Button(_routingTable.AlternativeRoute<SelectQuestionaireCommand>()),
                }
            }
            )
        });
    }
}
