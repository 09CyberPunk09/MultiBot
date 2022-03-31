using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class TimeTrackingActivityRepository : Repository<TimeTrackingActivity>
    {
        public TimeTrackingActivityRepository(RelationalSchemaContext ctx) : base(ctx)
        {

        }
    }
}
