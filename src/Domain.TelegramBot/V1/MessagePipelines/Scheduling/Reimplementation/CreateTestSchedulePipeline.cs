using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Reimplementation
{
    [Route("/test_schedule")]
    public class CreateTestSchedulePipeline : MessagePipelineBase
    {
        public CreateTestSchedulePipeline(ILifetimeScope scope) : base(scope)
        {
            IntegrateChunkPipeline<CreateSchedulePipelineChunk>();
        }
    }
}
