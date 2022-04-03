using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Persistence.Common.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : AuditableEntity
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
