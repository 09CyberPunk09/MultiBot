using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
	[Obsolete]
	public class HelloMessagePipeline : MessagePipelineBase
	{
		
		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			//Stages.Add(SayHello);
			//Stages.Add(SayWhatsUp);
			//Stages.Add(SayGoodbye);

			Current = Stages.First();
			CurrentActionIndex = 0;
		}

		//private IContentResult SayHello(IMessageContext ctx) => new TextResult() { TextMessage = "1.Hello" };
		//private IContentResult SayGoodbye(IMessageContext ctx) => new TextResult() { TextMessage = "3.Bye" };
		//private IContentResult SayWhatsUp(IMessageContext ctx) => new TextResult() { TextMessage = "2.What's Up?" };

	}
}
