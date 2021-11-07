using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
    [Route("/hello")]
	[Description("This is an endpoint for developers, we use it for confirming that everythinf is ok")]
	public class HelloMessagePipeline : MessagePipelineBase
	{
		public override void RegisterPipelineStages()
		{
			Stages.Add(SayHello);
			Stages.Add(SayWhatsUp);
			Stages.Add(SayGoodbye);
		}

        private ContentResult SayHello(MessageContext ctx)
        {
            return Text("1.Hello");
        }

        private ContentResult SayGoodbye(MessageContext ctx)
        {
            return Text("3.Bye");
        }

        private ContentResult SayWhatsUp(MessageContext ctx)
        {
            return Text("2.What's up?");
        }
    }
}
