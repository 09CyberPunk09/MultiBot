using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using System.Linq;

namespace Persistence.Master.Repositories;

public class UserRepositry : RelationalSchemaRepository<User>
{
    public UserRepositry(RelationalSchemaContext context) : base(context)
    { }

    public override IQueryable<User> GetQuery() =>
         GetTable()
                .Include(x => x.FeatureFlags)
                .Include(x => x.TelegramLogIns);

}