using Common.BaseTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Persistence.Common.DataAccess;
using System;
using System.Linq;

namespace Persistence.Sql
{
    public class LifeTrackerDbContext : RelationalSchemaContext
    {

        public LifeTrackerDbContext() : base()
        {
        }

        public DbContextOptions<LifeTrackerDbContext> _options;

        public override int SaveChanges()
        {
            using var syncDb = new SynchronizationDbContext();
            var data = ChangeTracker.Entries()
                         .Where(x => x.State == EntityState.Added ||
                                x.State == EntityState.Deleted ||
                                x.State == EntityState.Modified)
                         .ToList();
            data.ForEach(x =>
            {
                var entity = x.Entity as AuditableEntity;
                syncDb.EntityChanges.Add(new()
                {
                    EntityId = entity.Id,
                    TimeStamp = DateTime.Now,
                    ObjectContent = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    EntityState = x.State,
                    Id = Guid.NewGuid(),
                    TypeName = entity.GetType().FullName
                });
            });
            base.SaveChanges();

            return syncDb.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
            var configurationBuilder = new ConfigurationBuilder()
             .AddJsonFile("appSettings.json");
            var config = configurationBuilder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("LifeTrackerDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}