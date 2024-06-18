using Application.TelegramBot.Commands.Pipelines.Notes;
using Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

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
