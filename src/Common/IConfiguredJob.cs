using Quartz;
using System.Collections.Generic;

namespace Common;

public interface IConfiguredJob
{
    Dictionary<string, string> AdditionalData { get; set; }

    ITrigger GetTrigger();

    IJobDetail BuildJob();
}

public interface IConfiguredJob<TPayload> : IConfiguredJob
{
    TPayload Payload { get; init; }
}