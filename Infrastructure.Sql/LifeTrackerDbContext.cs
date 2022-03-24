using Common.BaseTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence.Common.DataAccess;
using System;
using System.Linq;

namespace Persistence.Sql
{
    public class LifeTrackerDbContext : RelationalSchemaContext
    {
        //todo: put into config.json
        private static readonly string connectionstring = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog= MultiBot-Db;";

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
                    ObjectContent = JsonConvert.SerializeObject(entity),
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
            optionsBuilder.UseSqlServer(connectionstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}