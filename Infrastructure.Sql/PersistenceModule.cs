using Autofac;
using Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Sql
{
	public class PersistenceModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SqlServerDbContext>().InstancePerLifetimeScope();
			base.Load(builder);
		}
	}
}
