using Application.Services;
using Autofac;
using Domain;
using Infrastructure.Jobs.Executor;
using Infrastructure.TelegramBot.Jobs;
using Persistence.Caching.Redis;
using Persistence.Sql;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class TelegramBotHandlerInstance
    {
        private IContainer _container;

        private MessageHandler _messageConsumer;

        private void ConfigureApplication()
        {
            var containerBuilder = new ContainerBuilder();

            ConfigureHandlersAccess(containerBuilder);

            ConfigurePersistence(containerBuilder);

            ConfigureDomain(containerBuilder);

            ResolveRequiredServices(containerBuilder);
        }

        public void Start()
        {
            ConfigureApplication();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void ResolveRequiredServices(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder.Build();

            _messageConsumer = _container.Resolve<MessageHandler>();
        }

        private void ConfigureHandlersAccess(ContainerBuilder containerBuilder)
        {
            _ = containerBuilder.RegisterType<MessageHandler>().SingleInstance();
        }

        private void ConfigurePersistence(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PersistenceModule>();
            _ = builder.RegisterModule<CachingModule>();
        }

        private void ConfigureDomain(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PipelinesModule>();
            _ = builder.RegisterModule<DomainModule>();
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
    }
}
