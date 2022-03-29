using Common.Entites;

namespace Persistence.Sql.Repositories
{
    public class TimeTrackingActivityRepository : Repository<TimeTrackingActivity>
    {
        public TimeTrackingActivityRepository(LifeTrackerDbContext ctx) : base(ctx)
        {

        }
    }
}
