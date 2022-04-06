using Autofac;
using Common.Entites;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Extensions;
using Persistence.Common.DataAccess.Interfaces;

namespace Persistence.Caching.SqlLite
{
    public class SqlLiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<SqlLiteDbContext>()
            .InstancePerLifetimeScope();

            _ = RelationalSchemaContext.RegisterRepositories(builder,typeof(SqlLiteRepositoryBase<>));
              
            base.Load(builder);
        }
    }
}
