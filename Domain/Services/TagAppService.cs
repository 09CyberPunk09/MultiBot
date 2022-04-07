using Common.Entites;
using Microsoft.EntityFrameworkCore;
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

        public void InitializeBaseComponentsPerUser(Guid userId)
        {
            Create(Tag.FirstPriorityToDoTagName, userId);
            Create(Tag.SecondPriorityToDoTagName, userId);
            Create(Tag.DoneToDoTagName, userId);
        }

        public Tag Update(Tag tag)
        {
            return _tagRepository.Update(tag);
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
       
        public Tag Get(string text,Guid userId) 
            => _tagRepository
                            .GetTable()
                            .Include(x => x.Notes)
                            .FirstOrDefault(x => x.UserId == userId && x.Name == text);

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