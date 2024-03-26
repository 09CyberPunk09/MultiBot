using Common.BaseTypes;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Master
{
    public class RelationalSchemaRepository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity
    {
        protected readonly RelationalSchemaContext _context;

        public RelationalSchemaRepository(RelationalSchemaContext ctx)
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

        public virtual void Remove(TEntity entity) => RemoveAction(entity);
        public virtual void Remove(Guid id) => RemoveAction(_context.Set<TEntity>().Find(id));

        private void RemoveAction(TEntity entity)
        {
            entity.LastModificationDate = DateTime.Now;
            entity.IsDeleted = true;
            _context.Update(entity);
            _context.SaveChanges();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetQuery().ToList();
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public virtual TEntity Get(Guid id)
        {
            return GetQuery().FirstOrDefault(x => x.Id == id);
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
            return _context.Set<TEntity>();
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

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().FirstOrDefault(predicate);
        }

        public void RemovePhysically(Guid id)
        {
            var entity = GetTable().FirstOrDefault(x => x.Id == id);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }
    }
}