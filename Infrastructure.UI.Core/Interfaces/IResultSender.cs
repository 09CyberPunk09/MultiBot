using Infrastructure.Kernel;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IResultSender : IStartStop
	{
		int SendMessage(IContentResult message, IMessageContext ctx);
	}
}
