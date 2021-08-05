using Persistence.Core.BaseTypes;

namespace Persistence.Core.Entites
{
	public class Note : AuditableEntity
	{
		public string Text { get; set; }
	}
}
