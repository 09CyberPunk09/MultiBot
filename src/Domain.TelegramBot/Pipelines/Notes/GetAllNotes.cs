using Application.Services;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

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
