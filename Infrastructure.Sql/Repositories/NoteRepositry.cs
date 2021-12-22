using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class NoteRepositry : Repository<Note>
	{
		public NoteRepositry(SqlServerDbContext context) : base(context)
        {}

	}
}
