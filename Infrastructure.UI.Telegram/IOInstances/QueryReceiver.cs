using Autofac;
using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace Infrastructure.UI.TelegramBot.IOInstances
{
	public class QueryReceiver : IQueryReceiver
	{
		private bool _started = false;
		private readonly ITelegramBotClient _uiClient;
		private readonly IResultSender _sender;
		private readonly ILifetimeScope _lifetimeScope;

		public QueryReceiver(ITelegramBotClient uiClient,
							 IResultSender sender,
							 ILifetimeScope lifetimeScope) =>
		(_uiClient, _sender) = (uiClient, sender);

		public void Start()
		{
			_started = true;

			
		}

		public void Stop()
		{
			
		}

		public void ConsumeQuery(object query)
		{
			
		}
	}
}
