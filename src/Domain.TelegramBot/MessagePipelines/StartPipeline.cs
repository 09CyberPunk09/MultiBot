using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.ComponentModel;

namespace Infrastructure.TelegramBot.MessagePipelines
{
    [Route("/start")]
    [Description("Use this command for start the app or sign in")]
    public class StartPipeline : MessagePipelineBase
    {
        private readonly UserAppService _userService;

        public StartPipeline(UserAppService userService, ILifetimeScope scope) : base(scope)
        {
            _userService = userService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(Start);
            //todo: implement
            // IntegrateChunkPipeline<MenuPipeline>();
            IsLooped = true;
        }

        public ContentResult Start(MessageContext ctx)
        {
            var user = _userService.GetByTgId(ctx.RecipientChatId);
            if (user == null)
            {
                var newUuser = _userService.CreateFromTelegram("", ctx.RecipientChatId);
                newUuser.TelegramLoggedIn = true;
                _userService.Update(newUuser);

            }
            else
            {
                user.TelegramLoggedIn = true;
                _userService.Update(user);
            }

            return new()
            {
                Text = "Telegram User successfully logged in. Welcome! To get the menu, use the command /menu",
                //InvokeNextImmediately = true
            };
        }
    }
}