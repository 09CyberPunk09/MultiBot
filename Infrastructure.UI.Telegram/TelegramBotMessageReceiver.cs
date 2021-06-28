using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
	public class TelegramBotMessageReceiver : IMessageReceiver
	{
		ITelegramBotClient _uiClient;

		public TelegramBotMessageReceiver(ITelegramBotClient uiClient)
			=> (_uiClient) = (uiClient);

		public void ConsumeMessage(object message)
		{
			throw new NotImplementedException();
		}

		public void Start()
		{
			_uiClient.OnMessage += _uiClient_OnMessage;
			_uiClient.StartReceiving();
		}

		private void _uiClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
		{
			ConsumeMessage(e?.Message);
		}

		public void Stop()
		{
			_uiClient.StopReceiving();
			_uiClient.OnMessage -= _uiClient_OnMessage;
		}
	}
}
