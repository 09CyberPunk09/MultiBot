using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;
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
            IsLooped = true;
        }

        public ContentResult Start(MessageContext ctx)
        {
            var user = _userService.GetByTgId(ctx.Recipient);
            if (user == null)
            {
                //add pipeline for gettin name
                //TODO: Retrieve recipientUserId
                var newUuser = _userService.CreateFromTelegram("", ctx.RecipientUserId, ctx.Recipient);
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