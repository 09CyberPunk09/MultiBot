using Infrastructure.UI.Core.Interfaces;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class TextResult : IContentResult
	{
		public Message TextMessage { get; set; }
	}
}
