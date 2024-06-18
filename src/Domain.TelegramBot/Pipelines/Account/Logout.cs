using Application.Services.Users;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Account;

[Route("/logout")]
public class LogoutCommand : ITelegramCommand
{
    private UserAppService _userService;
    public LogoutCommand(UserAppService userService)
    {
        _userService = userService;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        _userService.TelegramLogOut(ctx.RecipientUserId);
        return ContentResponse.Text("Done");
    }
}
