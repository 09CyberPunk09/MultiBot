using Infrastructure.Kernel;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IMessageReceiver : IStartStop
	{
		void ConsumeMessage(object message);
	}
}
