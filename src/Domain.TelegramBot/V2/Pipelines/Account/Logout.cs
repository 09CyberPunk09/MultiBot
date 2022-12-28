using Application.Services.Users;
using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Application.TextCommunication.Core.Routing;
using Application.TextCommunication.Core.StageMap;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Account
{
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
}
