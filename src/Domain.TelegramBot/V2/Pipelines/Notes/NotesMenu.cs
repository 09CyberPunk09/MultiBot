using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TelegramBot.Pipelines.V2.Pipelines.TagsV2;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using static Application.TextCommunication.Core.Repsonses.Menu;

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
                Menu = new(MenuType.MenuKeyboard, new []
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
