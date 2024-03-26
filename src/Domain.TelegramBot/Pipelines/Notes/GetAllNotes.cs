using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Commands.Pipelines.Notes;

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
        var result = _noteService.GetAllByUserId(ctx.User.Id);
        StringBuilder sb = new();
        sb.AppendLine("Your notes:");
        sb.AppendLine();
        foreach (var note in result)
        {

        }
        return ContentResponse.New(new()
        {
            Text = "Here are your notes:"
        });
    }
}
