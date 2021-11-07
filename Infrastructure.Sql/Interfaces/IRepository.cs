using Persistence.Core.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Core
{
    public interface IRepository<TEntity> 
		where TEntity : AuditableEntity
	{
		int SaveChanges();
		TEntity Add(TEntity entity);
		IEnumerable<TEntity> GetAll();
		TEntity Get(Guid Id);
		TEntity Update(TEntity entity);
		IQueryable<TEntity> GetQuery();
    }
}
