using Autofac;
using Common;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Jobs.Executor
{
    public class JobExecutor : IJobExecutor
    {
        private readonly ILifetimeScope _scope;

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
        }

        public async Task StartExecuting()
        {
            var scheduler = _scope.Resolve<IScheduler>();
            await scheduler.Start().ConfigureAwait(true);
        }
    }
}