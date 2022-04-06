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