using Autofac;
using MediatR;
using Persistence.Core;
using Persistence.Core.Entites;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Features.Notes
{
	public class GetNoteRequest : IRequest<QueryResult<Note>>
	{
		public string Text { get; set; }
	}
	public class GetNoteHandler : IRequestHandler<GetNoteRequest, QueryResult<Note>>
	{
		private readonly IRepository<Note> _noterepository;
		public GetNoteHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_noterepository = scope.Resolve<IRepository<Note>>();
		}

		Task<QueryResult<Note>> IRequestHandler<GetNoteRequest, QueryResult<Note>>.Handle(GetNoteRequest request, CancellationToken cancellationToken)
		{
			var result = _noterepository.GetAll().ToList();
			return Task.FromResult(new QueryResult<Note>(result));
		}
	}
}
