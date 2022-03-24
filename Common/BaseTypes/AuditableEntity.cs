using System;

namespace Common.BaseTypes
{
    public class AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}