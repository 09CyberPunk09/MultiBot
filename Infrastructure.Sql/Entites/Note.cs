using Persistence.Core.BaseTypes;

namespace Persistence.Sql.Entites
{
    public class Note : AuditableEntity
	{
		public string Text { get; set; }
	}
}
