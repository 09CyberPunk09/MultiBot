using Common.Entites;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace Persistence.Sql.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(LifeTrackerDbContext context) : base(context)
        { }

        private IQueryable<Tag> TagQuery => _context.Tags
                                                    .Include(x => x.Notes);
        public override Tag Get(Guid id)
        {
            return TagQuery.FirstOrDefault(x => x.Id == id);
        }
    }
}
