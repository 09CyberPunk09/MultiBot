using Application.Services;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.TextCommunication.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.NotesV2
{
    [Route("/get-notes", "📋 Notes")]
    //TODO: think about reconstruction of notes. adding attachments,etc

    public class GetAllNotesCommand : ITelegramCommand
    {
        private readonly NoteAppService _noteService; 
        public GetAllNotesCommand(NoteAppService noteAppService)
        {
            _noteService = noteAppService;
        }
        public void DefineStages(StageMapBuilder builder)
        {
        }

        public Task<StageResult> Execute(TelegramMessageContext ctx)
        {
            var result = _noteService.GetByUserId(ctx.User.Id);
            //todo: add markup
            var messagesToSend = new List<ContentResultV2>();

            messagesToSend.AddRange(result.Select(x => new ContentResultV2()
            {
                Text = x.Text,
                Menu = new(MenuType.MenuKeyboard,
                        new[]
                        {
                            new[]
                            {
                                new Button("Delete","")
                            }
                        })
            }));

            return ContentResponse.New(new()
            {
                Text = "Here are your notes:",
                MultiMessages = messagesToSend
            });
        }
    }
}
