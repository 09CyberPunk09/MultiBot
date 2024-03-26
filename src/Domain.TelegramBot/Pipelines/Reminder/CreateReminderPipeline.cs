using Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder.Chunks;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder
{
    [Route("/create_reminder", "➕ Create Reminder")]
    public class CreateReminderPipeline : MessagePipelineBase
    {
        public CreateReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(AskName);
            IntegrateChunkPipeline<ScheduleReminderChunkPipeline>();
        }

        public ContentResult AskName()
            => Text("Enter reminder text:");

    }
}
