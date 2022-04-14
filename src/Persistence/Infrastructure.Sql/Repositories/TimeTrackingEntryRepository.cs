using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class TimeTrackingEntryRepository : LifeTrackerRepository<TimeTrackingEntry>
    {
        public TimeTrackingEntryRepository(RelationalSchemaContext ctx) : base(ctx)
        {

        }
    }
}
