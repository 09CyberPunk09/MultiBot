using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class PredefinedAnswerRepository : Repository<PredefinedAnswer>
    {
        public PredefinedAnswerRepository(RelationalSchemaContext context) : base(context)
        { }
    }
}