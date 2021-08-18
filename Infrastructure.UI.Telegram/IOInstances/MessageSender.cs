using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.TelegramBot.ResponseTypes;
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
		//TODO: change type to void
		public int SendMessage(IContentResult message,IMessageContext ctx)
		{
			void SendTextMessage(string text)
			{
				_uiClient.SendTextMessageAsync(
				chatId: ctx.Recipient as ChatId,
				text: text);
			}

			//TODO: Rewrite to more extensible way
			switch (message)
			{
				case TextResult textResult:
					SendTextMessage(textResult.TextMessage.Text);
					break;
				case MultiMessageResult multi:
					multi.Messages.ForEach(x => SendTextMessage(x.Text));
					break;
				default:
					throw new NotImplementedException("The method to concrete response type is not implemented!");
			}


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
