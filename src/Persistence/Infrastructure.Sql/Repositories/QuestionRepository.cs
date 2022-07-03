using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Master.Repositories
{
    public class QuestionRepository : LifeTrackerRepository<Question>
    {
        public QuestionRepository(RelationalSchemaContext context) : base(context)
        { }

        private IQueryable<Question> _questionQuery
        {
            get 
            { 
                return GetTable()
                                .Include(q => q.PredefinedAnswers); 
            }
        }

        public override Question Get(Guid id)
        {
            return _questionQuery
                .FirstOrDefault(q => q.Id == id);
        }

        public override IEnumerable<Question> GetAll()
        {
            return _questionQuery.AsEnumerable();
        }
    }
}