using Infrastructure.UI.Core.Interfaces;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class TextResult : IContentResult
	{
		public string Text { get; set; }
	}
}
