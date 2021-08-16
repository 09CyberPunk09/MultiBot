using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class DomainModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterMediatR(typeof(DomainModule).Assembly);
			base.Load(builder);
		}
	}
}
