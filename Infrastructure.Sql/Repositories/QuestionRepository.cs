using Common.Entites;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace Persistence.Sql.Repositories
{
    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository(LifeTrackerDbContext context) : base(context)
        { }

        public override Question Get(Guid id)
        {
            return GetTable().Include(q => q.PredefinedAnswers).FirstOrDefault(q => q.Id == id);
        }

    }
}
