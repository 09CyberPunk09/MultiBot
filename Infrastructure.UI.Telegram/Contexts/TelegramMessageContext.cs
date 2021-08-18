using Infrastructure.UI.Core.Interfaces;
using System;

namespace Infrastructure.UI.TelegramBot
{
	public class TelegramMessageContext : IMessageContext
	{
		public object Message { get; set; }
		public DateTime TimeStamp { get; set; }
		public bool MoveNext { get; set; }
		public object Recipient { get; set; }
	}
}
