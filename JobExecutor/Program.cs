using Autofac;
using Autofac.Extras.Quartz;
using Common;
using Infrastructure.TelegramBot.Jobs;
using NLog;
using Persistence.Common.DataAccess.Jobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Jobs.Executor
{
    internal class Program
    {
        private static ContainerBuilder _containerBuilder;
        private static List<Type> _configurationTypes = new();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            _containerBuilder = new();
            _containerBuilder.RegisterModule<JobExecutorModule>();
            logger.Info("Configuration of job executor started");

            StartJobs();

            var container = _containerBuilder.Build();
            var executor = container.Resolve<IJobExecutor>();
            _configurationTypes.ForEach(type =>
            {
                var obj = (IConfiguredJob)container.Resolve(type);
                executor.ScheduleJob(obj);

            });
            await executor.StartExecuting();
            LoopConsoleClosing();
        }

        private static void StartJobs()
        {
            AddJob<SynchronizationJobConfiguration>();
            AddJob<QustionSchedulingJobConfiguration>();
            AddJob<ReminderSchedulerJobConfiguration>();
        }

        private static void AddJob<TType>() where TType : IConfiguredJob
        {
            _configurationTypes.Add(typeof(TType));
            _containerBuilder.RegisterAssemblyModules(typeof(TType).Assembly);
            _containerBuilder.RegisterType<TType>();
            _containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(TType).Assembly));
        }

        private static void LoopConsoleClosing()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            { }
            Console.WriteLine("");
        }
    }
}