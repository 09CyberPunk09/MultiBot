using Common.Entites.Questionaires;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System.Linq;

namespace Persistence.Master.Repositories;

public class QuestionaireRepository : RelationalSchemaRepository<Questionaire>
{
    public QuestionaireRepository(RelationalSchemaContext ctx) : base(ctx)
    {

    }

    public override IQueryable<Questionaire> GetQuery()
    {
        return GetTable()
            .Include(x => x.Questions)
            .ThenInclude(x => x.PredefinedAnswers);
    }
}
