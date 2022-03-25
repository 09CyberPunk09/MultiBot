using Common.SynchronizationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Common.DataAccess
{
    public class SynchronizationDbContext : DbContext
    {
        public DbSet<EntityChange> EntityChanges { get; set; }

        public SynchronizationDbContext() : base()
        {
        }

        public DbContextOptions<SynchronizationDbContext> _options;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configurationBuilder = new ConfigurationBuilder()
                      .AddJsonFile("appSettings.json");
            var config = configurationBuilder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("SynchronizationDb"));
        }
    }
}