using Common.Entites;
using Common.Entites.Questionaires;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Master.Repositories;

public class QuestionaireRepository : RelationalSchemaRepository<Questionaire>
{
    public QuestionaireRepository(RelationalSchemaContext ctx) : base(ctx) 
    {

    }

    public override Questionaire Get(Guid id)
    {
        return GetQuery()
            .Include(x => x.Questions)
            .ThenInclude(x => x.PredefinedAnswers)
            .FirstOrDefault(x => x.Id == id);
    }
}
