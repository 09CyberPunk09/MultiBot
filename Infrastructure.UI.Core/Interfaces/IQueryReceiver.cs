using Infrastructure.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IQueryReceiver : IStartStop
	{
		void ConsumeQuery(object query);
	}
}
