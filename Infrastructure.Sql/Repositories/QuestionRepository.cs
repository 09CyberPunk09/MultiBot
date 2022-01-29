using Microsoft.EntityFrameworkCore;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Linq;

namespace Persistence.Sql.Repositories
{
    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository(SqlServerDbContext context) : base(context)
        { }

        public override Question Get(Guid id)
        {
            return GetTable().Include(q => q.PredefinedAnswers).FirstOrDefault(q => q.Id == id);
        }

    }
}
