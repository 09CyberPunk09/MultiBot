using Common.Entites;
using System.Collections.Generic;

namespace Persistence.Sql.Repositories
{
    public class UserRepositry : Repository<User>
    {
        public UserRepositry(LifeTrackerDbContext context) : base(context)
        { }
        public override IEnumerable<User> GetAll()
        {
            return base.GetAll();
        }
    }
}
