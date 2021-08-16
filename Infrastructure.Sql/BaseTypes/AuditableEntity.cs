using System;

namespace Persistence.Core.BaseTypes
{
	public class AuditableEntity
	{
		public Guid Id { get; set; }
		public DateTime LastModification { get; set; }
	}
}
