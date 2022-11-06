using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.ComponentModel;

namespace Domain.TelegramBot.MessagePipelines.Hello
{
    [Route("/hello")]
    [Description("This is an endpoint for developers, we use it for confirming that everythinf is ok")]
    public class HelloMessagePipeline : MessagePipelineBase
    {
        public HelloMessagePipeline(ILifetimeScope scope) : base(scope)
        {
         //   IntegrateChunkPipeline<HelloMessagePipelineChunk>();
            RegisterStageMethod(SayHello);
          //  IntegrateChunkPipeline<HelloMessagePipelineChunk>();
            RegisterStageMethod(SayWhatsUp);
          //  IntegrateChunkPipeline<HelloMessagePipelineChunk>();
            RegisterStageMethod(SayGoodbye);
          //  IntegrateChunkPipeline<HelloMessagePipelineChunk>();
        }


        private ContentResult SayHello()
        {
            return Text("1.Hello");
        }

        private ContentResult SayGoodbye()
        {
            return Text("3.Bye");
        }

        private ContentResult SayWhatsUp()
        {
            return Text("2.What's up?");
        }
    }
}