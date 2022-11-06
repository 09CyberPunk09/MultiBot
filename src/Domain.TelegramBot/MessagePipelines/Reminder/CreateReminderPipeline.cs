using Autofac;
using Domain.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/create_reminder", "➕ Create Reminder")]
    public class CreateReminderPipeline : MessagePipelineBase
    {
        public CreateReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(AskName);
            this.IntegrateChunkPipeline<ScheduleReminderChunkPipeline>();
        }

        public ContentResult AskName()
            => Text("Enter reminder text:");

    }
}
