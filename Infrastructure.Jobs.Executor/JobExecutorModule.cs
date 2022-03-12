using Autofac;
using Autofac.Extras.Quartz;
using System.Collections.Specialized;

namespace Infrastructure.Jobs.Executor
{
    //todo: refactor for not to be dependent from one assembly or create a separate project woth jobs
    public class JobExecutorModule : Module
    {
        private readonly System.Reflection.Assembly _assemblyWithJobs;
        public JobExecutorModule(System.Reflection.Assembly assemblyWithJobs)
        {
            _assemblyWithJobs = assemblyWithJobs;
        }
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

            builder.RegisterModule(new QuartzAutofacJobsModule(_assemblyWithJobs));

            builder.RegisterType<JobExecutor>().SingleInstance();
        }
    }
}
