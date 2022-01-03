using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;

namespace Persistence.Sql.Repositories
{
    public class UserRepositry : Repository<User>
    {
        public UserRepositry(SqlServerDbContext context) : base(context)
        { }

    }
}
