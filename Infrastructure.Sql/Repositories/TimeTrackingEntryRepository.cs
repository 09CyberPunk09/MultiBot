using Common.Entites;
using Persistence.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Sql.Repositories
{
    public class TimeTrackingEntryRepository : Repository<TimeTrackingEntry>
    {
        public TimeTrackingEntryRepository(RelationalSchemaContext ctx) : base(ctx)
        {

        }
    }
}
