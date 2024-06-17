using Newtonsoft.Json;
using Telegram.Bot.Types;
using TelegramBot.ChatEngine.Implementation.Dro;
using TelegramBot.ChatEngine.Setup;
//setting up the message handler

var builder = new MessageHandlerBuilder();
builder.Services.AddCommandsAndStages();
builder.MessageTransportation.RegisterSenderAction(resp =>
{
    Console.WriteLine(JsonConvert.SerializeObject(resp));
    return Task.FromResult(new SentTelegramMessage
    {
        SentMessage = new Message
        {
            MessageId = 11
        }
    });
});

var handler = builder.Build();
while (true)
{
    var textMessage = Console.ReadLine();
    await handler.HandleMessage(new TelegramBot.ChatEngine.Commands.TelegramMessage()
    {
        ChatId = 1,
        Text = textMessage,
        UserId = 1
    });
}