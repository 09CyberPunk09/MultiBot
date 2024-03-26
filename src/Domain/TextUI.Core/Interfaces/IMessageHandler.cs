using System.Threading.Tasks;

namespace Application.Chatting.Core.Interfaces
{
    public interface IMessageHandler
    {
        //TODO:FIX
        Task HandleMessage(TelegramMessage message);
        Task StartReceiving();
    }
}
