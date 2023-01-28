using System;
using System.Collections.Generic;

namespace Common.Entites.Scheduling;

public class ScheduleExpressionDto
{
    public const string CACHEKEY = "SchedulingMode";
    public ScheduleExpressionDto(List<string> crons)
    {
        Crons = crons;
        FireAndForget = false;
    }

    public ScheduleExpressionDto(List<DateTime> fireAndForgetDates)
    {
        FireAndForgetDates = fireAndForgetDates;
        FireAndForget = true;
    }

    /// <summary>
    /// For deserialization only
    /// </summary>
    public ScheduleExpressionDto()
    {

    }
    //TODO: !ADD DESCRIPTIONS TO PIPELINES
    public string Description { get; set; }
    public int Mode { get; set; }
    public List<string> Crons { get; set; } = new();
    public bool FireAndForget { get; set; } = false;
    public List<DateTime> FireAndForgetDates { get; set; } = new();
}
