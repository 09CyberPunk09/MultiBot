using Persistence.Core.BaseTypes;
using System;

namespace Persistence.Sql.Entites
{
    public class Set : AuditableEntity
    {
        public string Name { get; set; }
      //  public Guid UserId { get; set; }
    }
}
