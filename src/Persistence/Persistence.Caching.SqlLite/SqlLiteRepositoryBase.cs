using Common.BaseTypes;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Caching.SqlLite
{
    public class SqlLiteRepositoryBase<T> : IRepository<T> where T : AuditableEntity
    {
        private readonly SqlLiteDbContext _context;
        public SqlLiteRepositoryBase(SqlLiteDbContext ctx)
        {
            _context = ctx;
        }
        public T Add(T entity)
        {
            var result = _context.Set<T>().Add(entity).Entity;
            _context.SaveChanges();
            return result;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Add(entity);
            }
            _context.SaveChanges();
            return entities;
        }

        public long Count()
        {
            return _context.Set<T>().Count();
        }

        public T FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }

        public void Remove(T entity)
        {
            entity.IsDeleted = true;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void RemovePhysically(T entity)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            var res = _context.Set<T>().Update(entity).Entity;
            _context.SaveChanges();
            return res;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Update(entity);
            }
            _context.SaveChanges();
            return entities;
        }
    }
}
