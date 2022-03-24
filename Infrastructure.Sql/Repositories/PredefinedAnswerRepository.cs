using Common.Entites;

namespace Persistence.Sql.Repositories
{
    public class PredefinedAnswerRepository : Repository<PredefinedAnswer>
    {
        public PredefinedAnswerRepository(LifeTrackerDbContext context) : base(context)
        { }
    }
}