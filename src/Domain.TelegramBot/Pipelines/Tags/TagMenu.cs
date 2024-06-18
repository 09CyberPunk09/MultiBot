using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;

[Route("/tag_menu", "📂 Tags")]
public class TagMenuCommand : ITelegramCommand
{
    private readonly RoutingTable _routingTable;
    public TagMenuCommand(RoutingTable routingTable)
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
            Text = "Tags menu",
            Menu = new(MenuType.MenuKeyboard, new[]
            {
               new[]
               {
                   new Button(_routingTable.AlternativeRoute<AddTagCommand>()),
                   new Button(_routingTable.AlternativeRoute<GetAllTagsCommand>()),
               },
               [
                   new Button(_routingTable.AlternativeRoute<GettagDataCommand>()),
                   new Button(_routingTable.AlternativeRoute<AddNoteUnderTagCommand>()),
               ]
            })
        });
    }
}