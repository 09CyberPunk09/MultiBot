using Infrastructure.Kernel;
using Infrastructure.UI.Core.MessagePipelines;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IMessageReceiver : IStartStop
	{
		void ConsumeMessage(object message);
	}
}
