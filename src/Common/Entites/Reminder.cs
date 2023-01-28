using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class Reminder : SchedulerInfoEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string SchedulerExpression { get; set; }
        public bool IsActive { get; set; }
    }
}
