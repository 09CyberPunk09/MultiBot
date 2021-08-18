using Infrastructure.Kernel;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IHandlerInstance : IStartStop 
	{
		IResultSender Sender { get; }
		IMessageReceiver Receiver { get; }
	}
}
