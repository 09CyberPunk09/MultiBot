using Autofac;
using Common.Enums;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using TextUI.Core.PipelineBaseKit;

namespace Domain.TelegramBot.MessagePipelines.Hello
{
    [Route("/hello")]
    [FeaureFlags(FeatureFlag.BasicFunctionality)]
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