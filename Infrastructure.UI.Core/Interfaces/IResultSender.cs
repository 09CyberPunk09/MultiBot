using Infrastructure.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IResultSender : IStartStop
	{
		int SendMessage(IContentResult message);
	}
}
