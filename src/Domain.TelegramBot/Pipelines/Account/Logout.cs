using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Users;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Account
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
