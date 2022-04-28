using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class TimeTrackingEntry : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTime StartTime { get; set; }
        public bool Completed { get; set; }
        public DateTime EndTime { get; set; }
    }
}
