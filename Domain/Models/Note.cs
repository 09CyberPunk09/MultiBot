using Infrastructure.Kernel;

namespace Domain.Models
{
	public class Note : AuditableObject
	{
		public string Text { get; set; }
	}
}
