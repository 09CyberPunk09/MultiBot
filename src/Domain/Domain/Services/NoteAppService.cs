using Common.Entites;
using Persistence.Common.DataAccess.Interfaces;
using Persistence.Common.DataAccess.Interfaces.Repositories;
using System;
using System.Collections.Generic;

namespace Application.Services;

public class NoteAppService : AppService
{
    private readonly INoteRepository _noteRepository;
    private readonly IRepository<Tag> _tagRepository;

    public NoteAppService(
        INoteRepository noteRepository,
        IRepository<Tag> tagRepository
        )
    {
        _noteRepository = noteRepository;
        _tagRepository = tagRepository;
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

    public Note Create(Guid userId, string text, string fileHash = null)
    {
        return _noteRepository.Add(new()
        {
            Text = text,
            UserId = userId,
            FileLink = fileHash
        });
    }

    public void Remove(Guid id)
    {
        _noteRepository.Remove(id);
    }

    public IEnumerable<Note> GetAllByUserId(Guid userId)
    {
        return _noteRepository.GetAllByUserId(userId);
    }

    public void TagNote(Guid noteId, Guid tagId)
    {
        var note = _noteRepository.Get(noteId);
        var tag = _tagRepository.Get(tagId);
        note.Tags.Add(tag);
        _noteRepository.Update(note);
    }
}
