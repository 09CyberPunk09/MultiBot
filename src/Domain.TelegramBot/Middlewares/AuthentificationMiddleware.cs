using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Routing;
using Application.Telegram.Implementations;
using Application.TelegramBot.Commands.Core;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Implementations.Middlewares;
using Application.TelegramBot.Commands.Pipelines.Account;
using Application.TelegramBot.Pipelines.V2.Pipelines.Account;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Middlewares;

public class AuthentificationMiddleware : ITelegramMiddleware
{
    private readonly IMessageSender<SentTelegramMessage> _sender;
    private readonly RoutingTable _routingTable; 
    public AuthentificationMiddleware(IMessageSender<SentTelegramMessage> sender, RoutingTable routingTable)
    {
        _sender = sender;
        _routingTable = routingTable;
    }

    public Task<bool> ExecuteAsync(TelegramMessageContext context)
    {
        var text = context.Message.Text;
        var command = _routingTable.GetCommand(text);
        var startCommandRoute = _routingTable.Route<TelegramStart>();
        var registerCommandRoute = _routingTable.Route<RegisterCommand>();
        if (command != null && 
            command.Route.Route != startCommandRoute && 
            command.Route.Route != registerCommandRoute)
        {
            var userExists = context.User != null;
            if (!userExists)
            {
                _sender.SendMessageAsync(new AdressedContentResult()
                {
                    Text = "Hey! Looks like you are not recognized by the system. Sign in or register to continue with the bot😉. Use /start for sign in and /register for sign up.",
                      ChatId = context.RecipientChatId,
                });
            }
            return Task.FromResult(userExists);
        }
        else
        {
            return Task.FromResult(true);
        }

    }
}
