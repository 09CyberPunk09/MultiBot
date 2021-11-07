using Autofac;
using MediatR;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System.Threading;
using System.Threading.Tasks;
namespace Domain.Features.Notes
{
    public class CreateNoteRequest : IRequest<SuccessResult>
	{
		public string Text { get; set; }
	}

	public class NoteCreatedHandler : IRequestHandler<CreateNoteRequest, SuccessResult>
	{
		private Repository<Note> _noteRepository;
		public NoteCreatedHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_noteRepository = scope.Resolve<Repository<Note>>();
		}
		Task<SuccessResult> IRequestHandler<CreateNoteRequest, SuccessResult>.Handle(CreateNoteRequest request, CancellationToken cancellationToken)
		{
			_noteRepository.Add(new Note() { Text = request.Text });
			_noteRepository.SaveChanges();
			return Task.FromResult(new SuccessResult());
		}
	}
}
