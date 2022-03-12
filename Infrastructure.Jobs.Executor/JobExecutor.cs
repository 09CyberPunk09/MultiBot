using Autofac;
using Autofac.Extras.Quartz;
using Infrastructure.Jobs.Core;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Jobs.Executor
{
    public class JobExecutor
    {
        private readonly ILifetimeScope _scope;
        public JobExecutor(ILifetimeScope scope)
        {
            _scope = scope;
        }
        public async Task ScheduleJob(IConfiguredJob configuredJob)
        {
           // QuartzAutofacFactoryModule
            Console.WriteLine("This sample demonstrates how to integrate Quartz and Autofac.");
            try
            {
                var cts = new CancellationTokenSource();
                var job = configuredJob.BuildJob();

                var trigger = configuredJob.GetTrigger();       

                var scheduler = _scope.Resolve<IScheduler>();
                await scheduler.ScheduleJob(job, trigger, cts.Token).ConfigureAwait(true);

            }
            catch (Exception ex)
            {
            }
        }

        public async Task StartExecuting()
        {
            var scheduler = _scope.Resolve<IScheduler>();
            await scheduler.Start().ConfigureAwait(true);

        }
    }
}