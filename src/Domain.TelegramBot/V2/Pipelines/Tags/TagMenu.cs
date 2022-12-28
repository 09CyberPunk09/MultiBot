using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

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
               new[]
               {
                   new Button(_routingTable.AlternativeRoute<GettagDataCommand>()),
                   new Button(_routingTable.AlternativeRoute<AddNoteUnderTagCommand>()),
               }
            })
        });
    }
}