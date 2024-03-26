using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Master.Repositories;

public class NoteRepositry : RelationalSchemaRepository<Note>, INoteRepository
{
    public NoteRepositry(RelationalSchemaContext context) : base(context)
    { }

    public override IQueryable<Note> GetQuery()
        => _context
            .Notes
            .Include(n => n.Tags);


    public List<Note> GetAllByUserId(Guid userId)
    {
        return GetQuery()
                .Where(x => x.UserId == userId)
                .ToList();
    }
}