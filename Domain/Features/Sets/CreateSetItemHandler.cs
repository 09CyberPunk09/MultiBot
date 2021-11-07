using Autofac;
using MediatR;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Features.Sets
{
    public class AddSetItemRequest : IRequest<SuccessResult>
    {
        public string Content { get; set; } 
        public Guid SetId { get; set; } 
    }
    public class CreateSetItemHandler : IRequestHandler<AddSetItemRequest, SuccessResult>
    {
        private readonly Repository<SetItem> _listNoteRepo;
        public CreateSetItemHandler()
        {
            var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
            _listNoteRepo = scope.Resolve<Repository<SetItem>>();
        }

        public Task<SuccessResult> Handle(AddSetItemRequest request, CancellationToken cancellationToken)
        {
            _listNoteRepo.Add(new SetItem()
            {
                 Name = request.Content,
                 SetId = request.SetId
            });
            return Task.FromResult(new SuccessResult());
        }
    }
}
