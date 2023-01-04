using Ardalis.SmartEnum;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Enums
{
    public class SchedulingMode : SmartEnum<SchedulingMode>
    {
        /*
                 * Every day at [time]
                 * Every [days] at [time]
                 * Every weekend at [time]
                 * On [date] at [time]
                 * Every [N]th day starting from today [time]
                 * Every [Month] on [day] at [time]
                 * CRON
        */

        public static readonly SchedulingMode EveryDayAT = new(1, "Every day at [time],...[timeN]");
        public static readonly SchedulingMode EceryDAYSAT = new(2, "Every [days] at [time],...[timeN]");
        public static readonly SchedulingMode EveryWEEKENDAT = new(3, "Every weekend at [time],...[timeN]");
        public static readonly SchedulingMode OnDATEAT = new(4, "On [date] at [time],...[timeN]");
        public static readonly SchedulingMode EveryNTHDAYStartingFromTodayAT = new(5, "Every [N]th day starting from 1st of a month [time]");
        public static readonly SchedulingMode EveryMONTHonDAYAT = new(6, "Every [Month] on [day] at [time],...[timeN]");
        public static readonly SchedulingMode CRON = new(7, "CRON");

        public SchedulingMode(int value, string name, string example = "") : base(name, value)
        {
            Example = example;
        }

        public string Example { get; set; }
    }
}
