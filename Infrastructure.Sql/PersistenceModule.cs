using Autofac;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using Persistence.Sql.Repositories;

namespace Persistence.Sql
{
    public class PersistenceModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			_ = builder.RegisterType<SqlServerDbContext>().InstancePerLifetimeScope();

			_ = builder.RegisterType<NoteRepositry>().As<Repository<Note>>().InstancePerLifetimeScope();
			_ = builder.RegisterType<SetRepository>().As<Repository<Set>>().InstancePerLifetimeScope();
			_ = builder.RegisterType<SetItemRepository>().As<Repository<SetItem>>().InstancePerLifetimeScope();
			base.Load(builder);
		}
	}
}
