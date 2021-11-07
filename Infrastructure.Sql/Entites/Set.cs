using Persistence.Core.BaseTypes;
using System.Collections.Generic;

namespace Persistence.Sql.Entites
{
    public class Set : AuditableEntity
    {
        public string Name { get; set; }
        public List<SetItem> ListNotes { get; set; }
    }

}
