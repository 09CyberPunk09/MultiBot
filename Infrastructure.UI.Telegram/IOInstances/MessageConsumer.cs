using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot.IOInstances
{
    public class MessageConsumer
    {
		#region Injected members
		private readonly ITelegramBotClient _uiClient;
		private readonly IResultSender _sender;
		private readonly ILifetimeScope _lifetimeScope;
		#endregion

		private static readonly Dictionary<string, Type> pipleineCommands;
		public IMessagePipeline DefaultPipeline { get; set; }

        public MessageConsumer(IResultSender sender,
							   ILifetimeScope lifetimeScope,
							   ITelegramBotClient uiClient)
        {
			_sender = sender;
			_lifetimeScope = lifetimeScope;
			_uiClient = uiClient;
        }


        static MessageConsumer()
        {
			//TODO:  make attribute multiparametrized,duplicate in list every entry which has more than one command
			pipleineCommands = GetPipelineTypes().ToDictionary(x => (x.GetCustomAttributes(true).FirstOrDefault(attr => (attr as RouteAttribute) != null) as RouteAttribute).Route);
		}

		public void ConsumeMessage(Message message)
		{
			MessageContext ctx = new()
			{
				Message = message,
				MoveNext = true,
				Recipient = message.ChatId,
				TimeStamp = DateTime.Now
			};
			//try
			//{
			if (DefaultPipeline == null)
				DefaultPipeline = MatchPipeline(message.Text);
			if (DefaultPipeline != null)
			{
				ExecutePieplineStage(ctx);
			}
			//}
			//catch (Exception ex)
			//{
			//	throw;
			//}

		}

		//TODO: decompose this object to Sending base and query and  message derivings
		public void ExecutePieplineStage(MessageContext ctx)
		{
			if (DefaultPipeline != null)
			{
				var result = DefaultPipeline.ExecuteCurrent(ctx);
				_sender.SendMessage(result, ctx);
			}
		}


		private MessagePipelineBase MatchPipeline(string text)
		{
			var matchedPipelineType = pipleineCommands.ToList().FirstOrDefault(x => text.Contains(x.Key)).Value;
		
			return matchedPipelineType != null ?_lifetimeScope.BeginLifetimeScope().Resolve(matchedPipelineType) as MessagePipelineBase : null;
		}

		private static List<Type> GetPipelineTypes()
		{
			var basePipelineType = typeof(MessagePipelineBase);
			return typeof(MessageReceiver).Assembly.GetTypes().Where(t => t.IsSubclassOf(basePipelineType)).ToList();
		}


	}
}
