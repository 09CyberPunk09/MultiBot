﻿using Autofac;
using Autofac.Extras.Quartz;
using Common;
using Integration.Applications;
using NLog;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Application.Chatting.Core.Interfaces;
using Application.TelegramBot.Commands.Core;
using Application.TelegramBot.Commands.Implementations.Infrastructure;
using Application.TelegramBot.Commands;
using Application;
using Common.Configuration;
using System.Configuration;
using Application.TelegramBot.Commands.Jobs;
using Telegram.Bot;

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