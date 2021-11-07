using Infrastructure.Kernel;
using Infrastructure.UI.Core.Types;

namespace Infrastructure.UI.Core.Interfaces
{
    public interface IMessageReceiver : IStartStop
	{
		void ConsumeMessage(Message message);
    }
}
