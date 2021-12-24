using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class TagAppService : AppService
    {
        private Repository<Tag> _tagRepository;
        private Repository<Note> _noteRepository;
        public TagAppService(Repository<Tag> tagRepo,Repository<Note> noteRepository)
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
            =>_tagRepository.GetQuery().Where(x => x.UserId == userId);

        public Note CreateNoteUnderTag(Guid tagId,string text,Guid userId)
        {
            var tag = _tagRepository.Get(tagId);
            var note = _noteRepository.Add(new()
            {
                Text = text,
                Tags = new() { tag }
            });
            return note;
        }
    }
}
