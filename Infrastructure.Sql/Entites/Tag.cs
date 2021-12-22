using Persistence.Core.BaseTypes;
using System;

namespace Persistence.Sql.Entites
{
    public class Tag : AuditableEntity
    {
        public Guid? UserId { get; set; }
        public string Name { get; set; }
    }
}
