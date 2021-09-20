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
    public class GetUserSetsRequest : IRequest<QueryResult<Set>>
    {

    }
	public class GetUserSetsHandler : IRequestHandler<GetUserSetsRequest, QueryResult<Set>>
	{
		private IRepository<Set> _setRepository;
		public GetUserSetsHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_setRepository = scope.Resolve<IRepository<Set>>();
		}
		async Task<QueryResult<Set>> IRequestHandler<GetUserSetsRequest, QueryResult<Set>>.Handle(GetUserSetsRequest request, CancellationToken cancellationToken)
		{
			var result = _setRepository.GetAll().ToList();
			return new QueryResult<Set>(result);
		}
	}


}
