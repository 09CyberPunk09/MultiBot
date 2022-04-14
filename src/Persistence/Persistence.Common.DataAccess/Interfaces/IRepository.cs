using Common.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Common.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : AuditableEntity
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
        void RemovePhysically(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetQuery();
        long Count();
        TEntity FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
