using Common.SynchronizationEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common.DataAccess
{
    public class SynchronizationDbContext : DbContext
    {
        private static readonly string connectionstring = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog= SynchronizationDb;";
        public DbSet<EntityChange> EntityChanges { get; set; }

        public SynchronizationDbContext() : base()
        {
        }

        public DbContextOptions<SynchronizationDbContext> _options;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionstring);
        }
    }
}