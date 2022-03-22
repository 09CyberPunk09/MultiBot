using Autofac;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.TelegramBot.MessagePipelines
{
    [Route("/hello")]
    [Description("This is an endpoint for developers, we use it for confirming that everythinf is ok")]
    public class HelloMessagePipeline : MessagePipelineBase
    {
        public HelloMessagePipeline(ILifetimeScope scope) : base(scope)
        {

        }
        public override void RegisterPipelineStages()
        {
            RegisterStage(SayHello);
            RegisterStage(SayWhatsUp);
            RegisterStage(SayGoodbye);
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
