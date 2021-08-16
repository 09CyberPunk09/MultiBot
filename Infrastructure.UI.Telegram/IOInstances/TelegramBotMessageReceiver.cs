using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using System;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
	public class TelegramBotMessageReceiver : IMessageReceiver
	{
		ITelegramBotClient _uiClient;
		IResultSender _sender;

		public TelegramBotMessageReceiver(ITelegramBotClient uiClient, IResultSender sender)
		{
			(_uiClient, _sender) = (uiClient, sender);
			DefaultPipeline.IsLooped = true;
			DefaultPipeline.RegisterPipelineStages();

		}
		public IMessagePipeline DefaultPipeline = new HelloMessagePipeline();
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
