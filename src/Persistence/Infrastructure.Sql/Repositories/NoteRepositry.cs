using Common.Entites;
using Persistence.Common.DataAccess;

namespace Persistence.Master.Repositories
{
    public class NoteRepositry : LifeTrackerRepository<Note>
    {
        public NoteRepositry(RelationalSchemaContext context) : base(context)
        { }
    }
}