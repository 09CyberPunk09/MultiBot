using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Kernel
{
	public class AuditableObject
	{
		public Guid Id { get; set; }
		public DateTime DateCreated { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? LastModified { get; set; }

	}
}
