using Common.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Persistence.Common.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : AuditableEntity
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(Guid id);
        void RemovePhysically(Guid id);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
        long Count();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }
}
