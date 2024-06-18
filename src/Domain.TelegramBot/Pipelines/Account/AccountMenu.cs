using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Account;

[Route("/account_menu","Account")]
//TODO: IMPLEMENT
public class AccountMenuCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Not implemented yet");
    }
}
