using Common.BaseTypes;
using Common.Enums;
using System;

namespace Common.Entites
{
    public class TimeTrackingEntry : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTime TimeStamp { get; set; }
        public EntryType EntryType { get; set; }
    }
}
