using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Master.Repositories
{
    public class TimeTrackingActivityRepository : LifeTrackerRepository<TimeTrackingActivity>
    {
        public TimeTrackingActivityRepository(RelationalSchemaContext ctx) : base(ctx)
        {

        }
    }
}
