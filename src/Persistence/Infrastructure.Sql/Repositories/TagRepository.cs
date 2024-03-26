using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System.Linq;

namespace Persistence.Master.Repositories;

public class TagRepository : RelationalSchemaRepository<Tag>
{
    public TagRepository(RelationalSchemaContext context) : base(context)
    { }

    public override IQueryable<Tag> GetQuery() => _context.Tags
                                                .Include(x => x.Notes);
}