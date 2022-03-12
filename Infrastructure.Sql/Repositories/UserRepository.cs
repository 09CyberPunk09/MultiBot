using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System.Collections.Generic;

namespace Persistence.Sql.Repositories
{
    public class UserRepositry : Repository<User>
    {
        public UserRepositry(SqlServerDbContext context) : base(context)
        { }
        public override IEnumerable<User> GetAll()
        {
            return base.GetAll();
        }
    }
}
