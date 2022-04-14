using Common.BaseTypes;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Sql
{
    public class LifeTrackerRepository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity
    {
        protected readonly RelationalSchemaContext _context;

        public LifeTrackerRepository(RelationalSchemaContext ctx)
        {
            _context = ctx;
        }

        public virtual RelationalSchemaContext GetContext()
        {
            return _context;
        }

        public virtual TEntity Add(TEntity entity)
        {
            SetCreationAuditionFields(entity);
            entity.LastModificationDate = DateTime.Now;

            var result = _context.Add(entity).Entity;
            _context.SaveChanges();
            return result;
        }

        public virtual void Remove(TEntity entity)
        {
            entity.LastModificationDate = DateTime.Now;
            entity.IsDeleted = true;
            _context.Update(entity);
            _context.SaveChanges();
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
            entity.LastModificationDate = DateTime.Now;

            var result = _context.Update(entity).Entity;
            _context.SaveChanges();
            return result;
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual DbSet<TEntity> GetTable()
        {
            return _context.Set<TEntity>();
        }

        private static void SetCreationAuditionFields(AuditableEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.Id = Guid.NewGuid();
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Add(entity);
            }
            _context.SaveChanges();
            return entities;
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastModificationDate = DateTime.Now;
                _context.Update(entity);
            }
            _context.SaveChanges();
            return entities;
        }

        public long Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public TEntity FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void RemovePhysically(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
    }
}