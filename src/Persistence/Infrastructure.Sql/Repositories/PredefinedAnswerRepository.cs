using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Master.Repositories
{
    public class PredefinedAnswerRepository : LifeTrackerRepository<PredefinedAnswer>
    {
        public PredefinedAnswerRepository(RelationalSchemaContext context) : base(context)
        { }
    }
}