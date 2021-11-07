using System;

namespace Infrastructure.UI.Core.Types
{
    public class MessageContext
	{
		public Message Message { get; set; }
		public DateTime TimeStamp { get; set; }
		public bool MoveNext { get; set; }
		public long Recipient { get; set; }
    }
}
