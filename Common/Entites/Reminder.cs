using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class Reminder : SchedulerInfoEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public bool Recuring { get; set; }
        public DateTime? ReminderTime { get; set; }
        public string RecuringCron { get; set; }
    }
}
