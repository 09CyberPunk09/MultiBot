using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class AnswerRepository : LifeTrackerRepository<Answer>
    {
        public AnswerRepository(RelationalSchemaContext context) : base(context)
        { }
    }
}