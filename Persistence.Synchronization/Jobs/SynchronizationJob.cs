using Autofac;
using Common;
using NLog;
using Persistence.Caching.SqlLite;
using Persistence.Common.DataAccess;
using Persistence.Sql;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Synchronization.Jobs
{
    public class SynchronizationJobConfiguration : IConfiguredJob
    {
        public IJob Job { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; } = new();

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<SynchronizationJob>()
                                     .WithIdentity("datasync", "datasync");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            .WithIdentity("datasync-trigger", "datasync")
            .StartNow()
            .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(120)).Build();
        }
    }

    public class SynchronizationJob : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SynchronizationJob()
        {
        }

        public virtual Task Execute(IJobExecutionContext context)
        {
            try
            {
                ContainerBuilder masterContainerBuilder = new();
                masterContainerBuilder.RegisterModule(new PersistenceModule(true));
                ContainerBuilder testContainerBuilder = new();
                testContainerBuilder.RegisterModule<SqlLiteModule>();
                StorageSynchronizer synchronizer = new(masterContainerBuilder.Build(), testContainerBuilder.Build());
                synchronizer.Synchronize();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            return Task.CompletedTask;
        }
    }
}