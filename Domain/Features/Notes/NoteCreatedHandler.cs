using MediatR;
using Persistence.Core;
using Persistence.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Domain.Features.Notes
{
	public class  CreateNoteRequest : IRequest<SuccessResult>
	{
		public string Text { get; set; }
	}

	public class NoteCreatedHandler : IRequestHandler<CreateNoteRequest, SuccessResult>
	{
		private IRepository<Note> _noteRepository;
		public NoteCreatedHandler(IRepository<Note> noterepository) =>
			(_noteRepository) = (noterepository);

		async Task<SuccessResult> IRequestHandler<CreateNoteRequest, SuccessResult>.Handle(CreateNoteRequest request, CancellationToken cancellationToken)
		{
			_noteRepository.Add(new Note() { Text = request.Text });
			await _noteRepository.SaveChangesAsync();
			return new SuccessResult();
		}
	}
}
