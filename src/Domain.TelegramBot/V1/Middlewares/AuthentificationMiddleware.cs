using Application.TelegramBot.Pipelines.Old.MessagePipelines.Start;
using Application.TextCommunication.Core.Interfaces;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis;
using System;
using System.Threading.Tasks;
using TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.Old.Middlewares;

internal class AuthentificationMiddleware : IMessageHandlerMiddleware
{
    private readonly IMessageSender _sender;
    public AuthentificationMiddleware(IMessageSender sender)
    {
        _sender = sender;
    }
    public async Task<bool> Execute(ILifetimeScope scope, MessageContext ctx)
    {
        if (ctx.CurrentPipeline.Type != typeof(StartPipeline) &&
            ctx.CurrentPipeline.Type != typeof(RegisterPipeline))
        {
            var cache = new Cache(DatabaseType.TelegramLogins);
            var user = cache.Get<Guid?>(ctx.Message.UserId.ToString());
            if (!user.HasValue)
            {
                _sender.SendMessage(new()
                {
                    Text = "Hey! Looks like you are not recognized by the system. Sign in or register to continue with the bot😉. Use /start for sign in and /register for sign up.",
                    //RecipientChatId = ctx.Message.ChatId,
                });
            }
            return user.HasValue;
        }
        else return true;
    }
}
