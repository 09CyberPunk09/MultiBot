using Autofac;
using Autofac.Extras.Quartz;
using Common;
using System.Collections.Specialized;

namespace LifeTracker.JobExecutor
{
    public class JobExecutorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var schedulerConfig = new NameValueCollection {
                {"quartz.threadPool.threadCount", "3"},
                {"quartz.scheduler.threadName", "MyScheduler"}
            };

            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });
            builder.RegisterType<JobExecutor>().As<IJobExecutor>().SingleInstance();
        }
    }
}