using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Core
{
	public interface IRepository<TEntity>
	{
		int SaveChanges();
		Task<int> SaveChangesAsync()
		{
			return Task.Run(() => SaveChanges());
		}
		void Add(TEntity entity);
		TEntity Get();
		IEnumerable<TEntity> GetAll();
		TEntity Find(Guid Id);
	}
}
