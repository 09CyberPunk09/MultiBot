using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class MultiMessageResult : IContentResult
	{
		public List<Message> Messages { get; set; }
	}
}
