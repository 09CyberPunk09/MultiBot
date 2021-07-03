using Infrastructure.UI.Core.Interfaces;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.UI.TelegramBot
{
	public class MessageSender : IResultSender
	{
		ITelegramBotClient _uiClient;

		public MessageSender(ITelegramBotClient uiClient)
		{
			_uiClient = uiClient;
		}
		public int SendMessage(IContentResult message,IPipelineContext ctx)
		{
			var chat = ctx.Recipient as ChatId;
			_uiClient.SendTextMessageAsync(
				chatId: chat, 
				text: message.Text as string);
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
