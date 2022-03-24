using Microsoft.EntityFrameworkCore;
using System;

namespace Common.SynchronizationEntities
{
    public class EntityChange
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TypeName { get; set; }
        public string ObjectContent { get; set; }
        public EntityState EntityState { get; set; }
    }
}