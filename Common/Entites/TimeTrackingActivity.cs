using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class TimeTrackingActivity : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}
