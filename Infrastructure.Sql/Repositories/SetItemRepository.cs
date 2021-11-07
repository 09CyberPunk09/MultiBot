using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class SetItemRepository : Repository<SetItem>
    {
        public SetItemRepository(SqlServerDbContext context): base(context)
        {
        }
    }
}
