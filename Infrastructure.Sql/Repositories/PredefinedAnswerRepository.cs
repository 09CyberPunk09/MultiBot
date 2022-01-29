using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class PredefinedAnswerRepository : Repository<PredefinedAnswer>
    {
        public PredefinedAnswerRepository(SqlServerDbContext context) : base(context)
        { }
    }
}
