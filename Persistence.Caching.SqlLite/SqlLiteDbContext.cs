using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;

namespace Persistence.Caching.SqlLite
{
    public class SqlLiteDbContext : RelationalSchemaContext
    {
        public SqlLiteDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\1\\source\\repos\\vasylklapatyi\\MultiBot\\CacheDb\\CacheDB.db;");
        }
    }
}
