using Autofac;
using MediatR;
using Persistence.Core;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Features.Sets
{
   	public class CreateSetRequest : IRequest<SuccessResult>
	{
		public string Name { get; set; }
	}

	public class CreateSetHandler : IRequestHandler<CreateSetRequest, SuccessResult>
	{
		private IRepository<Set> _setRepository;
		public CreateSetHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_setRepository = scope.Resolve<IRepository<Set>>();
		}
		async Task<SuccessResult> IRequestHandler<CreateSetRequest, SuccessResult>.Handle(CreateSetRequest request, CancellationToken cancellationToken)
		{
			_setRepository.Add(new Set() { Name = request.Name });
			await _setRepository.SaveChangesAsync();
			return new SuccessResult();
		}
	}
}
