using Application.Services;
using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/recurent_reminder")]
    public class MakeReminderRecurentPipeline : MessagePipelineBase
    {
        private readonly ReminderAppService _service;
        public MakeReminderRecurentPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<ReminderAppService>();

            IntegrateChunkPipeline<CreateScheduleChunk>();
            RegisterStage(AcceptSchedule);
        }

        public ContentResult AcceptSchedule(MessageContext ctx)
        {
            var cron = GetCachedValue<string>(CreateScheduleChunk.CRONEXPR_CACHEKEY,ctx.Recipient);
            var reminderId = GetCachedValue<string>(CreateReminderPipeline.REMINDERID_CACHEKEY,ctx.Recipient);
            var reminder = _service.Get(Guid.Parse(reminderId));

            //TODO: Add queue to schedule in runtime
            reminder.Recuring = true;
            reminder.RecuringCron = cron;
            _service.Update(reminder);

            return Text("✅Done.");
        }

    }
}
