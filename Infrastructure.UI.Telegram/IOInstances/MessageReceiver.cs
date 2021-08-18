using Autofac;
using Autofac.Core.Lifetime;
using Domain.Features.Notes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using System;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Domain;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.UI.TelegramBot
{
	public class MessageReceiver : IMessageReceiver
	{
		private readonly ITelegramBotClient _uiClient;
		private readonly IResultSender _sender;
		private readonly ILifetimeScope _lifetimeScope;
		public MessageReceiver(ITelegramBotClient uiClient, IResultSender sender, ILifetimeScope scope)
		{

			(_uiClient, _sender, _lifetimeScope) = (uiClient, sender,scope);
			DefaultPipeline = scope.Resolve<GetNotesPipeline>();
			DefaultPipeline.IsLooped = true;
			DefaultPipeline.RegisterPipelineStages();

		}
		public IMessagePipeline DefaultPipeline;
		public void ConsumeMessage(object message)
		{
			var tgMessage = message as Telegram.Bot.Types.Message;
			TelegramMessageContext ctx = new TelegramMessageContext()
			{
				Message = tgMessage.Text,
				MoveNext = true,
				Recipient = new Telegram.Bot.Types.ChatId(tgMessage.Chat.Id),
				TimeStamp = DateTime.Now
			};
			var result =  DefaultPipeline.ExecuteCurrent(ctx);
			_sender.SendMessage(result, ctx);
		}

		public void Start()
		{
			_uiClient.StartReceiving<MessageUpdateHandler>();
		}
	
		public void Stop()
		{
		}
	}

}
