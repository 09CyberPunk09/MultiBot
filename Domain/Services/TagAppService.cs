using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;

namespace Application.Services
{
    public class TagAppService : AppService
    {
        private Repository<Tag> _tagRepository;
        public TagAppService(Repository<Tag> tagRepo)
        {
            _tagRepository = tagRepo;
        }

        public Tag Create(string name, Guid userId)
        {
            return _tagRepository.Add(new()
            {
                Name = name,
                UserId = userId
            });
        }
    }
}
