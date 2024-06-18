using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Commands.Interfaces;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Implementation.Dro;

namespace TelegramBot.ChatEngine.Setup;

public class MessageTransportationBuilder
{
    public bool SenderDefined { get; private set; }
    private Func<ContentResultV2, Task<SentTelegramMessage>> _senderAction;
    public MessageTransportationBuilder()
    {
        SenderDefined = false;
    }
    public void RegisterSenderAction(Func<ContentResultV2, Task<SentTelegramMessage>> action)
    {
        _senderAction = action;
        SenderDefined = true;
    }

    internal Func<ContentResultV2, Task<SentTelegramMessage>> GetSenderAction(ServiceProvider serviceProvider)
    {
        if (!SenderDefined && (serviceProvider.GetService<IMessageSender>() is null))
            throw new InvalidOperationException("There is no sender action registered or defined. Register an implementation of IMessageSender interface using the service provider.");

        if (SenderDefined)
        {
            return _senderAction;
        }
        else
        {
            return async response =>
            {
                var sender = serviceProvider.GetService<IMessageSender>();
                var result = await sender.SendMessageAsync(response);
                return result;
            };
        }
    }
}