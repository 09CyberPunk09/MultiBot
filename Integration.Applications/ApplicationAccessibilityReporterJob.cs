using Common;
using Common.Infrastructure;
using Persistence.Caching.Redis;
using Quartz;

namespace Integration.Applications
{
    public class ApplicationAccessibilityReporterJobConfiguration : IConfiguredJob
    {
        public IJob Job { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; } = new();

        public ApplicationAccessibilityReporterJobConfiguration(string applicationName, Guid instanceId)
        {
            AdditionalData[ApplicationAccessibilityReporterJob.APPLICATIONNAME_KEY] = applicationName;
            AdditionalData[ApplicationAccessibilityReporterJob.APPLICATIONINSTANCEID_KEY] = instanceId.ToString();
        }

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<ApplicationAccessibilityReporterJob>()
                                     .WithIdentity("app-accessibility", "app-accessibility-setup");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            .WithIdentity("app-accessibility-trigger", "app-accessibility-setup")
            .StartNow()
            .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(1)).Build();
        }
    }

    public class ApplicationAccessibilityReporterJob : IJob
    {
        public const string APPLICATIONNAME_KEY = "ApplicationName";
        public const string APPLICATIONINSTANCEID_KEY = "ApplicationInstanceId";
        public Task Execute(IJobExecutionContext context)
        {
            var cache = new Cache(DatabaseType.ApplicationAccessibility);
            var dataObject = new ApplicationAccessibilityData()
            {
                ApplicationName = context.JobDetail.JobDataMap[APPLICATIONNAME_KEY] as string,
                ApplicationInstanceID = context.JobDetail.JobDataMap[APPLICATIONINSTANCEID_KEY] as string,
                LastIdleTime = DateTime.Now,
            };
            cache.Set(dataObject.ApplicationName, dataObject);
            return Task.CompletedTask;
        }
    }
}
