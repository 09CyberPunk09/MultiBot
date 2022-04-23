using Autofac;
using Persistence.Common.DataAccess;

namespace Persistence.Caching.SqlLite
{
    public class SqlLiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<SqlLiteDbContext>()
            .InstancePerLifetimeScope();

            _ = RelationalSchemaContext.RegisterRepositories(builder, typeof(SqlLiteRepositoryBase<>));

            base.Load(builder);
        }
    }
}
