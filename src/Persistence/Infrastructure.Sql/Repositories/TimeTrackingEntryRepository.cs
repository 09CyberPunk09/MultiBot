using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Master.Repositories
{
    public class TimeTrackingEntryRepository : LifeTrackerRepository<TimeTrackingEntry>
    {
        public TimeTrackingEntryRepository(RelationalSchemaContext ctx) : base(ctx)
        {

        }
    }
}
