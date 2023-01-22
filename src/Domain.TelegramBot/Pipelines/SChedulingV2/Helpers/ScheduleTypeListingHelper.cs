using Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Enums;
using Application.TelegramBot.Pipelines.V2.Pipelines.SChedulingV2.Pipelines;
using System;
using System.Collections.Generic;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Helpers;

public static class ScheduleTypeListingHelper
{
    public static IReadOnlyList<(SchedulingMode, Type)> Modes = new List<(SchedulingMode, Type)>
    {
        (SchedulingMode.EveryDayAT,typeof(EveryDaySchedulerCommand)),
        (SchedulingMode.EceryDAYSAT,typeof(EveryDaySAtCommand)),
        (SchedulingMode.EveryWEEKENDAT,typeof(WeekendSchedulerCommand)),
        (SchedulingMode.OnDATEAT,typeof(OnDateCommand)),
        (SchedulingMode.EveryNTHDAYStartingFromTodayAT,typeof(EveryNthDayCommand)),
        (SchedulingMode.EveryMONTHonDAYAT,typeof(EveryMONTHonDAYATCommand)),
        (SchedulingMode.CRON,typeof(CronScheduleCommand))
    };

}
