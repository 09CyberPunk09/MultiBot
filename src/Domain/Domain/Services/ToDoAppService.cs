using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class ToDoAppService : AppService
    {
        private readonly LifeTrackerRepository<ToDoItem> _todoItemRepository;
        private readonly LifeTrackerRepository<ToDoCategory> _todoCategoryRepository;
        public ToDoAppService(LifeTrackerRepository<ToDoItem> todoItemRepository,
                             LifeTrackerRepository<ToDoCategory> todoCategoryRepository)
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

        public void DeleteToDoItem(ToDoItem entity)
        {
            _todoItemRepository.Remove(entity);
        } 

        public void DeleteToDoCategory(Guid id)
        {
            _todoCategoryRepository.Remove(id);
        }

        public IEnumerable<ToDoCategory> GetAllCategories(Guid userId, bool includeItems)
        {
            //TODO: Add query filter for deleted
            var q = _todoCategoryRepository.GetQuery()
                                            .Where(c => c.UserId == userId && !c.IsDeleted);
            if (includeItems)
            {
                q = q.Include(c => c.ToDoItems.Where(i => !i.IsDeleted && !i.IsDone));
            }

            return q.AsEnumerable();
        }
    }
}
