using Application.Services;
using Autofac;
using Domain.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/create_reminder", "➕ Create Reminder")]
    public class CreateReminderPipeline : MessagePipelineBase
    {
        public CreateReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(AskName);
            this.IntegrateChunkPipeline<ScheduleReminderChunkPipeline>();
        }

        public ContentResult AskName(MessageContext ctx)
            => Text("Enter reminder text:");
        
    }
}
