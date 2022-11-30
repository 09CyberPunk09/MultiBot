using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System.Linq;

namespace Persistence.Master.Repositories;

public class QuestionRepository : RelationalSchemaRepository<Question>
{
    public QuestionRepository(RelationalSchemaContext context) : base(context)
    { }

    public override IQueryable<Question> GetQuery()
    {
        return GetTable().Include(q => q.PredefinedAnswers);
    }
}