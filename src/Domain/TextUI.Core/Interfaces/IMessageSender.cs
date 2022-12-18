using Application.TextCommunication.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.TextCommunication.Core.Interfaces;

public interface IMessageSender
{
    void SendMessage(ContentResultV2 result);
    Task SendMessageAsync(ContentResultV2 result);
}
