using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Threading.Tasks;

namespace Application.TextCommunication.Core.Interfaces
{
    public interface IMessageHandler
    {
        //TODO:FIX
        Task HandleMessage(TelegramMessage message);
        Task StartReceiving();
    }
}
