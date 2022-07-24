using Common.Entites;
using Persistence.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class NoteAppService : AppService
    {
        private readonly LifeTrackerRepository<Note> _noteRepository;

        public NoteAppService(LifeTrackerRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public Note Get(Guid id)
        {
            return _noteRepository.Get(id);
        }

        public Note Update(Guid id, string text)
        {
            var note = _noteRepository.Get(id);
            note.Text = text;
            return _noteRepository.Update(note);
        }

        public Note Create(string text, Guid userId)
        {
            return _noteRepository.Add(new()
            {
                Text = text,
                UserId = userId
            });
        }

        public void Remove(Guid id)
        {
            _noteRepository.Remove(id);
        }

        public void RemovePhysically(Note entity)
        {
            _noteRepository.RemovePhysically(entity);
        }

        public IEnumerable<Note> GetByUserId(Guid userId)
        {
            return _noteRepository.GetQuery().Where(n => n.UserId == userId).AsEnumerable();
        }
    }
}
