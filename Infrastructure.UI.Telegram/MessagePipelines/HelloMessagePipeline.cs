using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
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

		private IContentResult SayHello(IPipelineContext ctx) => new TextResult() { Text = "1.Hello" };
		private IContentResult SayGoodbye(IPipelineContext ctx) => new TextResult() { Text = "3.Bye" };
		private IContentResult SayWhatsUp(IPipelineContext ctx) => new TextResult() { Text = "2.What's Up?" };

	}
}
