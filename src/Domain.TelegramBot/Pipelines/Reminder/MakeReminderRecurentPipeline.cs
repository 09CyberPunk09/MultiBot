using Application.Services;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder.Chunks;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Chunks;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder
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

        public ContentResult AcceptSchedule()
        {
            var cron = GetCachedValue<string>(CreateScheduleChunk.CRONEXPR_CACHEKEY);
            var reminderId = GetCachedValue<string>(ScheduleReminderChunkPipeline.REMINDERID_CACHEKEY);
            var reminder = _service.Get(Guid.Parse(reminderId));

            reminder.Recuring = true;
            reminder.RecuringCron = cron;
            _service.Update(reminder);

            return Text("✅Done.");
        }

    }
}
