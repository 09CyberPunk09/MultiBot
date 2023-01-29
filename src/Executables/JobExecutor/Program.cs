using Application;
using Application.Chatting.Core.Interfaces;
using Application.TelegramBot.Commands;
using Application.TelegramBot.Commands.Core;
using Application.TelegramBot.Commands.Implementations.Infrastructure;
using Application.TelegramBot.Commands.Jobs;
using Application.TelegramBot.Commands.Jobs.Reminders;
using Common;
using Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTracker.JobExecutor
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider;
        private static void ConfigureJobExecutor(IServiceProvider provider)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            JobExecutorConfiguration.Scheduler = schedulerFactory.GetScheduler().Result;

            JobExecutorConfiguration.Scheduler.JobFactory = new JobFactory(provider);
            JobExecutorConfiguration.Scheduler.Start().Wait();
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            Quartz.Logging.LogProvider.IsDisabled = true;

            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IMessageSender<SentTelegramMessage>, TelegramMessageSender>();

            var configuration = ConfigurationHelper.GetConfiguration();
            string botToken = configuration["Telegram:BotAPIKey"];
            services.AddTelegramClient(botToken);
            services.AddHost();
            services.AddDomain();
            services.AddMappers();
            services.AddConfiguration(configuration);
            services.AddSettings();
            services.AddSingleton<IJobExecutor, JobExecutor>();

            //questionires
            services
                .AddSingleton<QuestionaireStartupLoader>()
                .AddScoped<SendQuestionaireJob>();
           
            //reminders
            services
                .AddSingleton<RemindersLoader>()
                .AddScoped<SendReminderJob>();

            //all additional services
            services
            .AddDomain()
            .AddMappers()
            .AddConfiguration()
            .AddSettings();
            _serviceProvider = services.BuildServiceProvider();
            ConfigureJobExecutor(_serviceProvider);

            var questionaireLoader = _serviceProvider.GetService<QuestionaireStartupLoader>();
            questionaireLoader.ScheduleJobsOnStartup();
            questionaireLoader.ScheduleJobsFromChannel();
            
            var reminderLoader = _serviceProvider.GetService<RemindersLoader>();
            reminderLoader.ScheduleJobsOnStartup();
            reminderLoader.ScheduleJobsFromChannel();


            var executor = _serviceProvider.GetService<IJobExecutor>();
            await executor.StartExecuting();
            LoopConsoleClosing();
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