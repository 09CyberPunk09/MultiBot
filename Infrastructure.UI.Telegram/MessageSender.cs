using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
	public class MessageSender : IResultSender
	{
		ITelegramBotClient _uiClient;

		public int SendMessage(IContentResult message)
		{

			return 0;
		}

		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}
