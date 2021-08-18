using Autofac;
using Persistence.Core;
using Persistence.Core.Entites;
using Persistence.Sql.Repositories;

namespace Persistence.Sql
{
	public class PersistenceModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			_ = builder.RegisterType<SqlServerDbContext>().InstancePerLifetimeScope();
			//todo: change to gettin all types from assembly which implements a iface
			_ = builder.RegisterType<NoteRepositry>().As<IRepository<Note>>().InstancePerLifetimeScope();
			base.Load(builder);
		}
	}
}
