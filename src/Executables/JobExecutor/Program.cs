using Autofac;
using Autofac.Extras.Quartz;
using Common;
using Infrastructure.TelegramBot.Jobs;
using Integration.Applications;
using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTracker.JobExecutor
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
            //AddJob<SynchronizationJobConfiguration>();
            AddJob<QustionSchedulingJobConfiguration>();
            AddJob<ReminderSchedulerJobConfiguration>();
            AddJob(new ApplicationAccessibilityReporterJobConfiguration("LifeTracker.JobExecutor", InstanceIdentifier.Identifier));
        }

        private static void AddJob<TType>() where TType : IConfiguredJob
        {
            _configurationTypes.Add(typeof(TType));
            _containerBuilder.RegisterAssemblyModules(typeof(TType).Assembly);
            _containerBuilder.RegisterType<TType>();
            _containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(TType).Assembly));
        }
        private static void AddJob<TType>(TType type) where TType : class
        {
            _configurationTypes.Add(typeof(TType));
            _containerBuilder.RegisterAssemblyModules(typeof(TType).Assembly);
            _containerBuilder.RegisterInstance(type);
            _containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(TType).Assembly));
        }

        private static void LoopConsoleClosing()
        {
             //while (Console.ReadKey().Key != ConsoleKey.Escape)
            //{ }
            //Console.WriteLine("");
            var ended = new ManualResetEventSlim();
            var starting = new ManualResetEventSlim();

            AssemblyLoadContext.Default.Unloading += ctx =>
            {
                System.Console.WriteLine("Unloding fired");
                starting.Set();
                System.Console.WriteLine("Waiting for completion");
                ended.Wait();
            };

            System.Console.WriteLine("Waiting for signals");
            starting.Wait();

            System.Console.WriteLine("Received signal gracefully shutting down");
            Thread.Sleep(5000);
            ended.Set();
        }
    }
}