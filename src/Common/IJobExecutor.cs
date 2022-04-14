using System;
using System.Threading.Tasks;

namespace Common
{
    public interface IJobExecutor
    {
        public Guid InstanceId { get; }

        Task ScheduleJob(IConfiguredJob configuredJob);

        Task StartExecuting();

    }
}