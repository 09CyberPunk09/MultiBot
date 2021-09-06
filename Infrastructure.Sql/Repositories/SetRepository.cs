using Persistence.Core;
using Persistence.Sql.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Sql.Repositories
{
    public class SetRepository : IRepository<Set>
    {
        private readonly SqlServerDbContext _context;
        public SetRepository(SqlServerDbContext context) =>
            (_context) = (context);
        public void Add(Set entity)
        {
            _context.Add(entity);
        }

        public Set Find(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Set Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Set> GetAll()
        {
            return _context.Sets.ToList();
        }

        public int SaveChanges()
        {
           return _context.SaveChanges();
        }
    }
}
