using Autofac;
using MediatR;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Features.Sets
{
    public class GetUserSetsRequest : IRequest<QueryResult<Set>>
    {

    }
	public class GetUserSetsHandler : IRequestHandler<GetUserSetsRequest, QueryResult<Set>>
	{
		private Repository<Set> _setRepository;
		public GetUserSetsHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_setRepository = scope.Resolve<Repository<Set>>();
		}
		async Task<QueryResult<Set>> IRequestHandler<GetUserSetsRequest, QueryResult<Set>>.Handle(GetUserSetsRequest request, CancellationToken cancellationToken)
		{
			var result = _setRepository.GetAll().ToList();
			return new QueryResult<Set>(result);
		}
	}


}
