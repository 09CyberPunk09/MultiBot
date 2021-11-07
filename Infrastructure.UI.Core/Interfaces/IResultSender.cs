using Infrastructure.Kernel;
using Infrastructure.UI.Core.Types;

namespace Infrastructure.UI.Core.Interfaces
{
    public interface IResultSender : IStartStop
	{
		void SendMessage(ContentResult message, MessageContext ctx);
	}
}
