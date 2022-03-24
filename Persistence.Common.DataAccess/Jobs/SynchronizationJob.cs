using Common;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Common.DataAccess.Jobs
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
        public SynchronizationJob()
        {

        }

        public virtual Task Execute(IJobExecutionContext context)
        {
            using var syncDb = new SynchronizationDbContext();

            var changes = syncDb.EntityChanges.ToList();

            return Task.CompletedTask;
        }
    }
}
