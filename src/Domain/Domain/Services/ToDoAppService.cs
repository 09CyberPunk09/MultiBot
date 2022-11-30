using Common.Entites;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services;

public class ToDoAppService : AppService
{
    private readonly IRepository<ToDoItem> _todoItemRepository;
    private readonly IRepository<ToDoCategory> _todoCategoryRepository;
    public ToDoAppService(IRepository<ToDoItem> todoItemRepository,
                         IRepository<ToDoCategory> todoCategoryRepository)
    { 
        _todoItemRepository = todoItemRepository;
        _todoCategoryRepository = todoCategoryRepository;
    }

    public ToDoCategory CreateCategory(Guid userId,string name)
    {
        return _todoCategoryRepository.Add(new()
        {
            UserId = userId,
            Name = name,
            ToDoItems = new()
        });
    }   
    public ToDoItem CreateItem(Guid userId,Guid categoryId, string text)
    {
        return _todoItemRepository.Add(new()
        {
            UserId = userId,
            Text = text,
            ToDoCategoryId = categoryId
        });
    }

    public ToDoCategory GetCategory(Guid categoryId)
    {
        return _todoCategoryRepository.Get(categoryId);
    }

    public ToDoItem GetToDoItem(Guid id)
    {
        return _todoItemRepository.Get(id);
    }  
    public ToDoItem UpdateToDoItem(ToDoItem entity)
    {
        return _todoItemRepository.Update(entity);
    }

    public void DeleteToDoItem(Guid id)
    {
        _todoItemRepository.Remove(id);
    } 

    public void DeleteToDoCategory(Guid id)
    {
        _todoCategoryRepository.Remove(id);
    }

    public IEnumerable<ToDoCategory> GetAllCategories(Guid userId)
    {
        var q = _todoCategoryRepository.Where(c => c.UserId == userId && !c.IsDeleted );

        return q;
    }
}
