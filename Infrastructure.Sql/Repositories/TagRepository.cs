using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class TagRepository : Repository<Tag>
	{
		public TagRepository(SqlServerDbContext context) : base(context)
		{ }

	}
}
