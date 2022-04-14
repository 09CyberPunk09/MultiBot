using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Sql.Repositories
{
    public class NoteRepositry : LifeTrackerRepository<Note>
    {
        public NoteRepositry(RelationalSchemaContext context) : base(context)
        { }
    }
}