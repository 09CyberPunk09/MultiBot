using Common.Entites;
using Persistence.Sql;
using Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class TagAppService : AppService
    {
        private LifeTrackerRepository<Tag> _tagRepository;
        private LifeTrackerRepository<Note> _noteRepository;

        public TagAppService(LifeTrackerRepository<Tag> tagRepo, LifeTrackerRepository<Note> noteRepository)
        {
            _tagRepository = tagRepo;
            _noteRepository = noteRepository;
        }

        public Tag Create(string name, Guid userId)
        {
            return _tagRepository.Add(new()
            {
                Name = name,
                UserId = userId
            });
        }

        public Tag Get(Guid id) => _tagRepository.Get(id);

        public IEnumerable<Tag> GetAll(Guid userId)
            => _tagRepository.GetQuery().Where(x => x.UserId == userId);

        public Note CreateNoteUnderTag(Guid tagId, string text, Guid userId)
        {
            var tag = _tagRepository.Get(tagId);
            var note = _noteRepository.Add(new()
            {
                Text = text,
                Tags = new() { tag },
                UserId = userId
            });
            return note;
        }
    }
}