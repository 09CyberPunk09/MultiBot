using Infrastructure.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Note: AuditableObject
	{
		public string Text { get; set; }
	}
}
