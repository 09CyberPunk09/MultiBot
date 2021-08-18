using Infrastructure.Kernel;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IQueryReceiver : IStartStop
	{
		void ConsumeQuery(object query);
	}
}
