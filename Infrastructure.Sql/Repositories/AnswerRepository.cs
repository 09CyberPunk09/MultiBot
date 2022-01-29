using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class AnswerRepository : Repository<Answer>
    {
        public AnswerRepository(SqlServerDbContext context) : base(context)
        { }
    }
}
