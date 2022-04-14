using Common.Entites;
using Persistence.Common.DataAccess;
using System.Collections.Generic;

namespace Persistence.Sql.Repositories
{
    public class UserRepositry : LifeTrackerRepository<User>
    {
        public UserRepositry(RelationalSchemaContext context) : base(context)
        { }

        public override IEnumerable<User> GetAll()
        {
            return base.GetAll();
        }
    }
}