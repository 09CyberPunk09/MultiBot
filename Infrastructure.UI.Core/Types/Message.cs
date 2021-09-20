using Infrastructure.UI.Core.Interfaces;

namespace Infrastructure.UI.Core.Types
{
	//review the deriving type
	public interface IMessage : IContentResult
	{
		public string Text { get; set; }
	}
}
