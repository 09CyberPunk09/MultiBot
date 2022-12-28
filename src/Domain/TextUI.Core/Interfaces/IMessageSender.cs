using Application.Chatting.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.Chatting.Core.Interfaces;

public interface IMessageSender
{
    void SendMessage(ContentResultV2 result);
    Task SendMessageAsync(ContentResultV2 result);
}
