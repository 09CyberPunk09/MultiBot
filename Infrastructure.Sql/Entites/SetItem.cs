using Persistence.Core.BaseTypes;
using System;

namespace Persistence.Sql.Entites
{
    public class SetItem : AuditableEntity
    {
        public Set Set { get; set; }
        public string Name { get; set; }
        public Guid SetId { get; set; }
        public short Number { get; set; }
    }

}
