using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class NoteRepositry : Repository<Note>
    {
        public NoteRepositry(RelationalSchemaContext context) : base(context)
        { }
    }
}