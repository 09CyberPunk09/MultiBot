using Autofac;
using MediatR;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
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
		private Repository<Set> _setRepository;
		public CreateSetHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_setRepository = scope.Resolve<Repository<Set>>();
		}
		Task<SuccessResult> IRequestHandler<CreateSetRequest, SuccessResult>.Handle(CreateSetRequest request, CancellationToken cancellationToken)
		{
			_setRepository.Add(new Set() { Name = request.Name });
			_setRepository.SaveChanges();
			return Task.FromResult(new SuccessResult());
		}
	}
}
