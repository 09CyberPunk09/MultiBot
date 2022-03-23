using Common.Entites;

namespace Persistence.Sql.Repositories
{
    public class NoteRepositry : Repository<Note>
    {
        public NoteRepositry(LifeTrackerDbContext context) : base(context)
        { }

    }
}
