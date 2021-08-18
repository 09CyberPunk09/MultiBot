using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class TextResult : IContentResult
	{
		public Message TextMessage { get; set; }
	}
}
