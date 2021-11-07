using Autofac;
using MediatR;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Features.Sets
{
    public class GetSetDataRequest : IRequest<QueryResult<SetItem>>
    {
        public string SetName { get; set; }
        public Guid? SetId { get; set; }
    }

    public class GetDataRequestHandler : IRequestHandler<GetSetDataRequest, QueryResult<SetItem>>
	{
		private Repository<SetItem> _listNoteRepository;
		private Repository<Set> _setRepository;
		public GetDataRequestHandler()
		{
			var scope = DependencyAccessor.LifetimeScope.BeginLifetimeScope();
			_setRepository = scope.Resolve<Repository<Set>>();
            _listNoteRepository = scope.Resolve<Repository<SetItem>>();
		}

        Task<QueryResult<SetItem>> IRequestHandler<GetSetDataRequest, QueryResult<SetItem>>.Handle(GetSetDataRequest request, CancellationToken cancellationToken)
        {
            var setName = request.SetName;
            Set existingSet = null;
            if(setName!= null)
                existingSet = _setRepository.GetAll().FirstOrDefault(x => setName.Contains(x.Name));
            else if(request.SetId != null)
                existingSet = _setRepository.GetAll().FirstOrDefault(x => x.Id == request.SetId.Value);
            if(existingSet != null)
            {
                var setDatas = _listNoteRepository.GetAll().Where(x => x.SetId == existingSet.Id).ToList();
                return Task.FromResult(new QueryResult<SetItem>(setDatas));
            }
            return Task.FromResult(new QueryResult<SetItem>(null));
        }
    }
}
