using System.Threading.Tasks;

namespace Common
{
    public interface IJobExecutor
    {
        Task ScheduleJob(IConfiguredJob configuredJob);
        Task StartExecuting();
    }
}
