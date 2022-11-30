using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis;
using System;

namespace Domain.TelegramBot.MessagePipelines.Start
{
    [Route("/logout")]
    public class LogOutPipeline : MessagePipelineBase
    {
        public LogOutPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(() =>
            {
                var userService = scope.Resolve<UserAppService>();
                userService.TelegramLogOut(MessageContext.RecipientUserId);

                return Text("Done");
            });
        }
    }
}
