using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Domain;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.UI.TelegramBot.IOInstances
{
	//todo: segregate into new file
	public class MessageUpdateHandler : IUpdateHandler
	{
		private static IMessageReceiver _messageReceiver;
		private static IQueryReceiver _queryReceiver;

		static MessageUpdateHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_messageReceiver = scope.Resolve<IMessageReceiver>();
			_queryReceiver = scope.Resolve<IQueryReceiver>();
		}
		public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			return null;
		}

		public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			switch (update.Type)
			{
				case UpdateType.Unknown:
					break;
				case UpdateType.Message:
					_messageReceiver.ConsumeMessage(update.Message);
					break;
				case UpdateType.InlineQuery:
					_queryReceiver.ConsumeQuery(update.InlineQuery);
					break;
				case UpdateType.ChosenInlineResult:
					break;
				case UpdateType.CallbackQuery:
					_queryReceiver.ConsumeQuery(update.CallbackQuery);
					break;
				case UpdateType.EditedMessage:
					break;
				case UpdateType.ChannelPost:
					break;
				case UpdateType.EditedChannelPost:
					break;
				case UpdateType.ShippingQuery:
					break;
				case UpdateType.PreCheckoutQuery:
					break;
				case UpdateType.Poll:
					break;
				case UpdateType.PollAnswer:
					break;
				case UpdateType.MyChatMember:
					break;
				case UpdateType.ChatMember:
					break;
				default:
					break;
			}
			return Task.FromResult(new object());
		}
	}
}
