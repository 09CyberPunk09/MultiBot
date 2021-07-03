using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IPipelineContext
	{
		object Message { get; set; }
		DateTime TimeStamp { get; set; }
		bool MoveNext { get; set; }
		object Recipient { get; set; }
	}
}
