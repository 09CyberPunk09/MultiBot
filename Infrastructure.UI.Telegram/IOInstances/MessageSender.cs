using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot
{
    public class MessageSender : IResultSender
	{
		ITelegramBotClient _uiClient;

		public MessageSender(ITelegramBotClient uiClient)
		{
			_uiClient = uiClient;
		}
		public void SendMessage(ContentResult message, MessageContext ctx)
		{
			void SendTextMessage(string text = "", IReplyMarkup markup = null)
			{
				_uiClient.SendTextMessageAsync(
				chatId: new ChatId(ctx.Message.ChatId),
				text: text,
				replyMarkup: markup);
			}

			//TODO: Rewrite to more extensible way
			switch (message)
			{
				case TextResult textResult:
					SendTextMessage(textResult.Text);
					break;
				case MultiMessageResult multi:
					multi.Messages.ForEach(x => SendTextMessage(x.Text));
					break;
				case BotMessage botMessage:
					SendTextMessage(botMessage.Text, botMessage.Buttons);
					break;
				case ContentResult textResult:
					SendTextMessage(textResult.Text);
					break;
				default:
					throw new NotImplementedException("The method to concrete response type is not implemented!");
			}
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
