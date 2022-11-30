using Autofac;
using Common;
using Integration.Applications;
using NLog;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTracker.JobExecutor
{
    public class JobExecutor : IJobExecutor
    {
        private readonly ILifetimeScope _scope;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public Guid InstanceId { get => InstanceIdentifier.Identifier; }
        public JobExecutor(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task ScheduleJob(IConfiguredJob configuredJob)
        {
            var cts = new CancellationTokenSource();
            var job = configuredJob.BuildJob();

            var trigger = configuredJob.GetTrigger();

            var scheduler = _scope.Resolve<IScheduler>();
            await scheduler.ScheduleJob(job, trigger, cts.Token).ConfigureAwait(true);
            logger.Info($"{configuredJob.GetType().Name} added to scheduler");
        }

        public async Task StartExecuting()
        {
            var scheduler = _scope.Resolve<IScheduler>();
            await scheduler.Start().ConfigureAwait(true);
            logger.Info($"Scheduler started");
        }
    }
}