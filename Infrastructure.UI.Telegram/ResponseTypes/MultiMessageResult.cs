using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using System.Collections.Generic;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class MultiMessageResult : IContentResult
	{
		public List<Message> Messages { get; set; }
	}
}
