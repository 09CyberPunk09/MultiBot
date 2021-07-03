using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot
{
	public class PipelineContext : IPipelineContext
	{
		public object Message { get ; set; }
		public DateTime TimeStamp { get; set; }
		public bool MoveNext { get ; set; }
		public object Recipient { get; set; }
	}
}
