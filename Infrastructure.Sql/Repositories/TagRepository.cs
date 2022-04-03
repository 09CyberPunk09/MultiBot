using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System;
using System.Linq;

namespace Persistence.Sql.Repositories
{
    public class TagRepository : LifeTrackerRepository<Tag>
    {
        public TagRepository(RelationalSchemaContext context) : base(context)
        { }

        private IQueryable<Tag> TagQuery => _context.Tags
                                                    .Include(x => x.Notes);

        public override Tag Get(Guid id)
        {
            return TagQuery.FirstOrDefault(x => x.Id == id);
        }
    }
}