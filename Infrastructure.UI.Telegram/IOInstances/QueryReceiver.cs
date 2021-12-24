using Autofac;
using Infrastructure.UI.Core.Interfaces;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot.IOInstances
{
    public class QueryReceiver : IQueryReceiver
	{
		private bool _started = false;
		private readonly ITelegramBotClient _uiClient;
		private readonly IResultSender _sender;
		private readonly ILifetimeScope _lifetimeScope;
		private readonly IMessageReceiver _receiver;
		private readonly MessageConsumer _messageConsumer;

        public QueryReceiver(ITelegramBotClient uiClient,
                             IResultSender sender,
                             ILifetimeScope lifetimeScope,
                             IMessageReceiver receiver, MessageConsumer consumer)
        {
            (_uiClient, _sender, _receiver, _messageConsumer) = (uiClient, sender, receiver, consumer);
        }

        public void Start()
		{
			_started = true;
		}

		public void Stop()
		{

		}

		public void ConsumeQuery(Core.Types.Message query)
		{
			_messageConsumer.ConsumeMessage(query);
		}
	}
}
