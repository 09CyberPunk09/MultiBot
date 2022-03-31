using System;

namespace Common.BaseTypes
{
    public class SchedulerInfoEntity : AuditableEntity
    {
        public Guid? SchedulerInstanceId { get; set; }
    }
}
