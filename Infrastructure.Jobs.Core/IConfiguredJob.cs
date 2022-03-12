using Quartz;
using System.Collections.Generic;

namespace Infrastructure.Jobs.Core
{
    public interface IConfiguredJob
    {
        Dictionary<string, string> AdditionalData { get; set; }
        IJob Job { get; set; }
        ITrigger GetTrigger();
        IJobDetail BuildJob();
    }
}
