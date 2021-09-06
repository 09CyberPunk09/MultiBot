using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.IOInstances;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.UI.TelegramBot
{
	public class MessageReceiver : IMessageReceiver
	{
		#region Injected members
		private readonly ITelegramBotClient _uiClient;
		private readonly IResultSender _sender;
		private readonly ILifetimeScope _lifetimeScope;
		#endregion

		private static readonly Dictionary<string, Type> pipleineCommands;
		public IMessagePipeline DefaultPipeline;

		static MessageReceiver()
		{
			//TODO:  make attribute multiparametrized,duplicate in list every entry which has more than one command
			pipleineCommands = GetPipelineTypes().ToDictionary(x=> (x.GetCustomAttributes(true).FirstOrDefault(attr => (attr as RouteAttribute) != null) as RouteAttribute).Route);

		}

		public MessageReceiver(ITelegramBotClient uiClient, IResultSender sender, ILifetimeScope scope)
		{

			(_uiClient, _sender, _lifetimeScope) = (uiClient, sender, scope);
			//DefaultPipeline = scope.Resolve<GetNotesPipeline>();
			//DefaultPipeline.IsLooped = true;
			//DefaultPipeline.RegisterPipelineStages();

		}
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
			try
			{
				if (DefaultPipeline == null)
					DefaultPipeline = MatchPipeline(tgMessage.Text);
				if (DefaultPipeline != null)
                {
					var result = DefaultPipeline.ExecuteCurrent(ctx);
					_sender.SendMessage(result, ctx);
                }
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public void Start()
		{
			//MatchPipeline();
			_uiClient.StartReceiving<MessageUpdateHandler>();
		}

		public void Stop()
		{
		}

		private MessagePipelineBase MatchPipeline(string text)
		{
			var matchedPipelineType = pipleineCommands.ToList().FirstOrDefault(x => text.Contains(x.Key)).Value;
			return _lifetimeScope.BeginLifetimeScope().Resolve(matchedPipelineType) as MessagePipelineBase;
		}

		private MessagePipelineBase RestorePipeline(string command,int methodId = 0)
		{
			Type restoredPipelineType;
			if (pipleineCommands.TryGetValue(command, out restoredPipelineType))
			{
				return _lifetimeScope.Resolve(restoredPipelineType) as MessagePipelineBase;
			}
			else throw new NullReferenceException($"Could not resolve a pipeline by an non-existent command. Command: {command}");
		}



		private static List<Type> GetPipelineTypes()
		{
			var basePipelineType = typeof(MessagePipelineBase);
			return typeof(MessageReceiver).Assembly.GetTypes().Where(t => t.IsSubclassOf(basePipelineType)).ToList();
		}

	}

}
