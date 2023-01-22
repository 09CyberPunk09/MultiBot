using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeTracker.JobExecutor;

public static class JobExecutorConfiguration
{
    public static IScheduler Scheduler;

}
