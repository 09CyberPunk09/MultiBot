using Autofac;
using Persistence.Common.DataAccess.Interfaces;
using System;

namespace Persistence.Common.DataAccess.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterRepository<TEntity>(this ContainerBuilder builder, Type repositoryType)
        {
            _ = builder.RegisterType(repositoryType.MakeGenericType(typeof(TEntity))).As(typeof(IRepository<>).MakeGenericType(typeof(TEntity))).InstancePerLifetimeScope();
            return builder;
        }
    }
}
