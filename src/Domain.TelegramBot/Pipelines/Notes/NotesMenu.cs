using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Notes;
using Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.NotesV2
{
    [Route("/notes", "📓 Notes")]
    public class NotesMenuCommand : ITelegramCommand
    {
        private readonly RoutingTable _routingTable;
        public NotesMenuCommand(RoutingTable routingTable)
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
                Text = "Menu of notes",
                Menu = new(MenuType.MenuKeyboard, new[]
                {
                    new[]
                    {
                        new Button(_routingTable.AlternativeRoute<AddNoteCommand>()),
                        new Button(_routingTable.AlternativeRoute<GetAllNotesCommand>())
                    },
                    new[]
                    {
                        new Button(_routingTable.AlternativeRoute<TagMenuCommand>())
                    },
                })
            });
        }
    }
}
