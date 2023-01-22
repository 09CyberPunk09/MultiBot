using Autofac;
using Common;
using Infrastructure.Queuing;
using Integration.Applications;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTracker.JobExecutor
{
    public class JobExecutor : IJobExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Guid InstanceId { get => InstanceIdentifier.Identifier; }
        public JobExecutor(IServiceProvider scope)
        {
            _serviceProvider = scope;
        }

        public async Task ScheduleJob(IConfiguredJob configuredJob)
        {
            var cts = new CancellationTokenSource();
            var job = configuredJob.BuildJob();

            var trigger = configuredJob.GetTrigger();

            var scheduler = JobExecutorConfiguration.Scheduler;
            await scheduler.ScheduleJob(job, trigger, cts.Token).ConfigureAwait(true);
            logger.Info($"{configuredJob.GetType().Name} added to scheduler");
        }

        public async Task StartExecuting()
        {
            var scheduler = JobExecutorConfiguration.Scheduler;
            await scheduler.Start().ConfigureAwait(true);
            logger.Info($"Scheduler started");
        }
    }
}