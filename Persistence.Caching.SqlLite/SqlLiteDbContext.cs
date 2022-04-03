using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess;

namespace Persistence.Caching.SqlLite
{
    public class SqlLiteDbContext : RelationalSchemaContext
    {
        public SqlLiteDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CacheDB.db;");
        }
    }
}
