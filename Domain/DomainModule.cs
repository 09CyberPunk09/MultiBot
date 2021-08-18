using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using System;

namespace Domain
{
	public class DomainModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			_ = builder.RegisterMediatR(GetType().Assembly);
			_ = builder.RegisterType<DependencyAccessor>().SingleInstance();
	
			base.Load(builder);
		}
	}
}
