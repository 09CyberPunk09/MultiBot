using Common.Entites;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services;

public class TagAppService : AppService
{
    private IRepository<Tag> _tagRepository;
    private IRepository<Note> _noteRepository;

    public TagAppService(
        IRepository<Tag> tagRepo, 
        IRepository<Note> noteRepository)
    {
        _tagRepository = tagRepo;
        _noteRepository = noteRepository;
    }

    public Tag Update(Tag tag)
    {
        return _tagRepository.Update(tag);
    }

    public Tag Create(string name, Guid userId,bool isSystem = false)
    {
        return _tagRepository.Add(new()
        {
            Name = name,
            UserId = userId,
            IsSystem = isSystem
        });
    }

    public Tag Get(Guid id) 
        => _tagRepository.Get(id);

    public Tag Get(string text, Guid userId)
        => _tagRepository.FirstOrDefault(x => x.Name == text && x.UserId == userId);

    public IEnumerable<Tag> GetAll(Guid userId)
        => _tagRepository.Where(x => x.UserId == userId);

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