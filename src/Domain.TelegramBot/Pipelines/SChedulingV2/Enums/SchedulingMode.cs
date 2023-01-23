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
                 * 
                 * On [Date] at [time...] (recurent)
                 * Today at
                 * Tomarrow at
                 * 
        */

        public static readonly SchedulingMode TodayAt = new(8, "Today at [time(s)]");
        public static readonly SchedulingMode TomorrowAt = new(9, "Tomorrow at [time(s)]");
        public static readonly SchedulingMode EveryDayAT = new(1, "Every day at [time(s)]");
        public static readonly SchedulingMode EceryDAYSAT = new(2, "Every [days] at [time(s)]");
        public static readonly SchedulingMode EveryWEEKENDAT = new(3, "Every weekend at [time(s)]");
        public static readonly SchedulingMode OnDATEAT = new(4, "On [date] at [time(s)]");
        public static readonly SchedulingMode EveryNTHDAYStartingFromTodayAT = new(5, "Every [N]th day starting from 1st of a month [time(s)]");
        public static readonly SchedulingMode EveryMONTHonDAYAT = new(6, "Every [Month] on [day] at [time(s)]");
        public static readonly SchedulingMode CRON = new(7, "CRON");

        public SchedulingMode(int value, string name, string example = "") : base(name, value)
        {
            Example = example;
        }

        public string Example { get; set; }
    }
}
