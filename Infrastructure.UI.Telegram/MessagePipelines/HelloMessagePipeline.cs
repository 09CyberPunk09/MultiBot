using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
	[Route("/hello")]
	[Description("This is an endpoint for developers, we use it for confirming that everythinf is ok")]
	public class HelloMessagePipeline : MessagePipelineBase
	{

		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			Stages.Add(SayHello);
			Stages.Add(SayWhatsUp);
			Stages.Add(SayGoodbye);

			Current = Stages.First();
			CurrentActionIndex = 0;
		}

		private IContentResult SayHello(IMessageContext ctx) => new TextResult() { TextMessage = new BotMessage() { Text = "1.Hello" } };
		private IContentResult SayGoodbye(IMessageContext ctx) => new TextResult() { TextMessage = new BotMessage() { Text = "3.Bye" } };
		private IContentResult SayWhatsUp(IMessageContext ctx) => new TextResult() { TextMessage = new BotMessage() { Text = "2.What's up?" } };

	}
}
