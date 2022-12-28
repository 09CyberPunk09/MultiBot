using Application.Chatting.Core.Interfaces;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Implementations.Middlewares;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Middlewares;

internal class AuthentificationMiddleware : ITelegramMiddleware
{
    private readonly IMessageSender _sender;
    public AuthentificationMiddleware(IMessageSender sender)
    {
        _sender = sender;
    }

    public Task<bool> ExecuteAsync(TelegramMessageContext context)
    {
        var userExists = context.User != null;
        if (!userExists)
        {
            _sender.SendMessage(new()
            {
                Text = "Hey! Looks like you are not recognized by the system. Sign in or register to continue with the bot😉. Use /start for sign in and /register for sign up.",
                //RecipientChatId = ctx.Message.ChatId,
            });
        }
        return Task.FromResult(userExists);
    }
}
