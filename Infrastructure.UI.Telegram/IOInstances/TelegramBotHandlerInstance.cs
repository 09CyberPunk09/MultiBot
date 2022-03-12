using Application.Services;
using Autofac;
using Autofac.Core;
using Domain;
using Infrastructure.Jobs.Executor;
using Infrastructure.TelegramBot.Jobs;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.TelegramBot.IOInstances;
using Microsoft.Extensions.Configuration;
using Persistence.Caching.Redis;
using Persistence.Sql;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
    public class TelegramBotHandlerInstance : IHandlerInstance
    {
        public IResultSender Sender { get; set; }

        public IMessageReceiver Receiver { get; }

        //todo: remove 
        private ContainerBuilder _containerBuider;


        private void ConfigureApplication()
        {
            _containerBuider = new ContainerBuilder();
            _containerBuider.RegisterModule(new JobExecutorModule(GetType().Assembly));

            //Telegram bot direct deps
            _ = _containerBuider.RegisterInstance<TelegramBotClient>(ConfigureApiClient()).As<ITelegramBotClient>().SingleInstance();

            ConfigureIOInfrastructure();

            ConfigurePersistence();

            ConfigureDomain();
        }

        private void ConfigureConfigurationAccess()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            _ = _containerBuider.RegisterInstance(configurationBuilder.Build()).SingleInstance();

        }

        public void Start()
        {
            ConfigureApplication();
            var container = _containerBuider.Build();

            StartJobs(container);
            MessageUpdateHandler.SetAccessObjects(container.BeginLifetimeScope());

            container.Resolve<IMessageReceiver>().Start();
            //container.Resolve<IResultSender>().Start();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void ConfigureIOInfrastructure()
        {
            _ = _containerBuider.RegisterType<MessageReceiver>().As<IMessageReceiver>().SingleInstance();
            _ = _containerBuider.RegisterType<MessageSender>().As<IResultSender>().SingleInstance();
            _ = _containerBuider.RegisterType<QueryReceiver>().As<IQueryReceiver>().SingleInstance();
            //todo: ad as<>() section
            _ = _containerBuider.RegisterType<MessageConsumer>().SingleInstance();

        }

        private void ConfigurePersistence()
        {
            _ = _containerBuider.RegisterModule<PersistenceModule>();
            _ = _containerBuider.RegisterModule<CachingModule>();

        }

        private void ConfigureDomain()
        {
            _ = _containerBuider.RegisterModule<PipelinesModule>();
            _ = _containerBuider.RegisterModule<DomainModule>();
        }

        private async Task StartJobs(IContainer container)
        {       
            var executor = container.Resolve<JobExecutor>();
            var userRepo = container.Resolve<Repository<User>>();
            var questionsToLoad = container.Resolve<QuestionAppService>()
                .GetScheduledQuestions();

            var users = userRepo.GetAll().ToList();

            questionsToLoad.ForEach(async q =>
            {
                var currentUser = users.FirstOrDefault(u => u.Id == q.UserId);
                //TODO: навести порядок з юзер айді і чат айді
                var dictionary = new Dictionary<string, string>
                {
                    { SendQustionJob.QuestionId, q.Id.ToString() },
                    { SendQustionJob.UserId, q.UserId.ToString() },
                    { SendQustionJob.ChatId, currentUser.TelegramUserId.ToString() }
                };
                await executor.ScheduleJob(new QuestionJobConfiguration()
                {
                    AdditionalData = dictionary
                });
            });

            await executor.StartExecuting();
        }

        private TelegramBotClient ConfigureApiClient()
        {
            var client = new TelegramBotClient("1740254100:AAGW32c6AWAqilo1xNYLUim5zsgTXn8g9x4") { Timeout = TimeSpan.FromMinutes(10) };

            client.GetUpdatesAsync();
            return client;
        }
    }
}
