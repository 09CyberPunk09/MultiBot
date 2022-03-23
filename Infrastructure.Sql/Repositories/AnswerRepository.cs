using Common.Entites;

namespace Persistence.Sql.Repositories
{
    public class AnswerRepository : Repository<Answer>
    {
        public AnswerRepository(LifeTrackerDbContext context) : base(context)
        { }
    }
}
