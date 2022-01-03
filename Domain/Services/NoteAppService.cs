using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class NoteAppService : AppService
    {
        private readonly Repository<Note> _noteRepository;
        public NoteAppService(Repository<Note> noteRepository)
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
