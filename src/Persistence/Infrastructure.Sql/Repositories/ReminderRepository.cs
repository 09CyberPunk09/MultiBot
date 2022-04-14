using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class ReminderRepository : LifeTrackerRepository<Reminder>
    {
        public ReminderRepository(RelationalSchemaContext ctx): base(ctx)
        {

        }
    }
}
