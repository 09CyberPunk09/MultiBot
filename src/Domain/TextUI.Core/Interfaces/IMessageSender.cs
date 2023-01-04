using Application.Chatting.Core.Messaging;
using Application.Chatting.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.Chatting.Core.Interfaces;

public interface IMessageSender
{

}
public interface IMessageSender<TResponse> : IMessageSender where TResponse : SentMessageRepsonse
{
    void SendMessage(ContentResultV2 result);
    Task<TResponse> SendMessageAsync(ContentResultV2 result) ;
}
