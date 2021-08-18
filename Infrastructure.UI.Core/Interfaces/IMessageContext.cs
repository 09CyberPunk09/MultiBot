using System;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IMessageContext
	{
		object Message { get; set; }
		DateTime TimeStamp { get; set; }
		bool MoveNext { get; set; }
		object Recipient { get; set; }
	}
}
