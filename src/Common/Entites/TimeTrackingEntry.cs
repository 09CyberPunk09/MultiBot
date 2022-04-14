using Common.BaseTypes;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
