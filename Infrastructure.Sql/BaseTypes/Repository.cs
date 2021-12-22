using Persistence.Core;
using Persistence.Core.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Sql.BaseTypes
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity
    {
        protected readonly SqlServerDbContext _context;
        public Repository(SqlServerDbContext ctx)
        {
            _context = ctx;
        }

        public virtual SqlServerDbContext GetContext()
        {
            return _context;
        }

        public virtual TEntity Add(TEntity entity)
        {
            SetCreationAuditionFields(entity);
            var result = _context.Add(entity).Entity;
            _context.SaveChanges();
            return result;
        }

        public virtual TEntity Find(Guid id)
        {
            return _context.Find<TEntity>(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public virtual TEntity Get(Guid id)
        {
            return _context.Find<TEntity>(id);
        }

        public virtual TEntity Update(TEntity entity)
        {
            var result = _context.Update(entity).Entity;
            _context.SaveChanges();
            return result;
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        static void SetCreationAuditionFields(AuditableEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.Id = Guid.NewGuid();
        }
    }
}
