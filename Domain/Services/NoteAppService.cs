using Common.Entites;
using Persistence.Sql;
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

        public Note Create(string text, Guid userId)
        {
            return _noteRepository.Add(new()
            {
                Text = text,
                UserId = userId
            });
        }

        public IEnumerable<Note> GetByUserId(Guid userId)
        {
            return _noteRepository.GetQuery().Where(n => n.UserId == userId).AsEnumerable();
        }
    }
}