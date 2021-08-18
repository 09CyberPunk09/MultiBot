using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	/// <summary>
	/// this type is created to access Autofac container without injecting the client type in it.
	/// </summary>
	public class DependencyAccessor
	{
		public static ILifetimeScope LifetimeScope { get; private set; }
		public DependencyAccessor(ILifetimeScope scope) =>
			LifetimeScope = scope;
	}
}
