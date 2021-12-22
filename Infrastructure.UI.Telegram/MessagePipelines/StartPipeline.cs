using Application;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
    [Route("/start")]
    [Description("Use this command for start the app or sign in")]
    public class StartPipeline : MessagePipelineBase
    {
        private readonly UserAppService _userService;
        public StartPipeline(UserAppService userService)
        {
            _userService = userService;
        }

        public override void RegisterPipelineStages()
        {
            Stages.Add(Start);
            IsLooped = true;
        }

        public ContentResult Start(MessageContext ctx)
        {
            var user = _userService.GetByTgId(ctx.Recipient);
            if (user == null)
            {
                //add pipeline for gettin name 
                var newUuser = _userService.CreateFromTelegram("", ctx.Recipient);
                newUuser.TelegramLoggedIn = true;
                _userService.Update(newUuser);
            }
            else
            {
                user.TelegramLoggedIn = true;
                _userService.Update(user);
            }
            return Text("Telegram User successfully logged in. Welcome!");
        }

    }
}
