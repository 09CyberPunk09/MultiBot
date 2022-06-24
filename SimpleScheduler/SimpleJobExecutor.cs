using Common;
using NLog;
using Quartz;

namespace SimpleScheduler
{
    /// <summary>
    /// Simple job scheduler for jobs which do not need container support
    /// </summary>
    public class SimpleJobExecutor : IJobExecutor
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IScheduler _scheduler;
        public SimpleJobExecutor()
        {
            Quartz.Logging.LogProvider.IsDisabled = true;
            _scheduler = SchedulerBuilder.Create().Build().GetScheduler().Result;
        }

        public Guid InstanceId { get; set; }

        public async Task ScheduleJob(IConfiguredJob configuredJob)
        {
            var cts = new CancellationTokenSource();
            var job = configuredJob.BuildJob();

            var trigger = configuredJob.GetTrigger();

            var scheduler = _scheduler;
            await scheduler.ScheduleJob(job, trigger, cts.Token).ConfigureAwait(true);
            logger.Info($"{configuredJob.GetType().Name} added to scheduler");
        }

        public async Task StartExecuting()
        {
            var scheduler = _scheduler;
            await scheduler.Start().ConfigureAwait(true);
            logger.Info($"Scheduler started");
        }
    }
}